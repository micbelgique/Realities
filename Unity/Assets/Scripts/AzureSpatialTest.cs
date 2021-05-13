using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.Azure.SpatialAnchors;
using UnityEngine.XR.ARFoundation;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;
using UnityEngine.UI;

public class AzureSpatialTest : MonoBehaviour
{

    #region Variables

    private CloudSpatialAnchor _savedAnchor;
    private bool startedAsa = false;
    private bool isErrorActive = false;
    public string model { get; set; }
    
    public int visibility, community;
    private AnchorLocateCriteria anchorLocateCriteria = null;
    private Text feedbackBox;
    private Slider progressBar;
    public bool hasSaved, isRotatingLeft, isRotatingRight, isScalingUp, isScalingDown;
    public delegate void MyEventHandler(GameObject e);
    public event MyEventHandler EventTriggered;
    #endregion // Private Variables

    #region Unity Inspector Variables
    [SerializeField] private SpatialAnchorManager cloudManager = null;
    [SerializeField] private string ApiURL;
    private CloudSpatialAnchorWatcher currentWatcher;

    private UIHandlerScript _uiHandler;

    #endregion // Unity Inspector Variables
    
    #region Properties
    public SpatialAnchorManager CloudManager { get { return cloudManager; } }
    private GameObject SpawnedObject { get; set; }
    public string userID { get; set; }
    public User currentUser { get; set; }
    private ReferencePointCreator ReferencePointCreator { get; set; }
    #endregion // Properties

    private void Awake()
    {
        CrossSceneInfoStatic.CurrentScene = "AR";
        if (string.IsNullOrEmpty(CrossSceneInfoStatic.CurrentUser))
        {
            currentUser = FileAndNetworkUtils.GetCurrentUser();
            if (currentUser is null)
            {
                CrossSceneInfoStatic.CurrentUser = "visitor";
                FileAndNetworkUtils.SaveElementToFile("user", "visitor");
            }
            else
                CrossSceneInfoStatic.CurrentUser = currentUser.id;
        }
    }

    void Start()
    {
        CrossSceneInfoStatic.Token = FileAndNetworkUtils.GetElementFromFile("token");
        
        ApiURL = Resources.Load<HIWConfig>("HIWConfig").apiBaseUrl;
        _uiHandler = this.GetComponent<UIHandlerScript>();

        StartCoroutine(StartLocation());
        
        if(!CrossSceneInfoStatic.CurrentUser.Equals("visitor"))
            currentUser = FileAndNetworkUtils.GetCurrentUser();

        feedbackBox = GameObject.Find("feedbackText").GetComponent<Text>();
        progressBar = GameObject.Find("progressBar").GetComponent<Slider>();
        progressBar.gameObject.transform.localScale = Vector3.zero;

        ReferencePointCreator = FindObjectOfType<ReferencePointCreator>();
        ReferencePointCreator.OnObjectPlacement += ReferencePointCreator_OnObjectPlacement;

        if (ARSession.state == ARSessionState.SessionTracking)
        {
            var startAzureSession = StartAzureSession();
        }
        else
        {
            ARSession.stateChanged += ARSession_stateChanged;
        }
    }

    private void Update()
    {
        if(isRotatingLeft)
            SpawnedObject.transform.Rotate(0, -3, 0);
        if(isRotatingRight)
            SpawnedObject.transform.Rotate(0, 3, 0);
        if (isScalingUp)
        {
            Vector3 currentScale = SpawnedObject.transform.localScale;
            SpawnedObject.transform.localScale = new Vector3(currentScale.x * 1.1f,currentScale.y * 1.1f,currentScale.z * 1.1f);
        }
        if (isScalingDown)
        {
            Vector3 currentScale = SpawnedObject.transform.localScale;
            SpawnedObject.transform.localScale = new Vector3(currentScale.x * 0.9f,currentScale.y * 0.9f,currentScale.z * 0.9f);
        }
    }

    #region AzureSpatialAnchors

    private void ARSession_stateChanged(ARSessionStateChangedEventArgs obj)
    {
        Debug.Log($"ar session {obj.state}");
        if (obj.state == ARSessionState.SessionTracking && !startedAsa)
        {
            var startAzureSession = StartAzureSession();
        }
    }

    public void SendScreenCapture()
    {
        ScreenCapture.TakeScreenShot_static(Screen.width, Screen.height, _savedAnchor.Identifier);
        _uiHandler.SetScreenCaptureVisible(false);
        feedbackBox.text = Texts.getScreenshotTaken() + "\n" +
                           Texts.getSelectAnotherModel();
        _uiHandler.SetPlusButtonActive(true);
    }

    public void SkipScreenshot()
    {
        _uiHandler.SetScreenCaptureVisible(false);
        feedbackBox.text = Texts.getScreenshotSkipped() + "\n" +
                           Texts.getSelectAnotherModel();
        _uiHandler.SetPlusButtonActive(true);
    }
    
    
    
    public async Task StartAzureSession()
    {
        startedAsa = true;
        anchorLocateCriteria = new AnchorLocateCriteria();
        
        CloudManager.SessionUpdated += CloudManager_SessionUpdated;
        CloudManager.AnchorLocated += CloudManagerOnAnchorLocated;
        CloudManager.LocateAnchorsCompleted += CloudManager_LocateAnchorsCompleted;
        CloudManager.LogDebug += CloudManagerOnLogDebug;
        CloudManager.Error += CloudManager_Error;
        
        await CloudManager.CreateSessionAsync();

        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ApiURL + $"/api/AnchorsAPI/near?Longitude={Input.location.lastData.longitude}&Latitude={Input.location.lastData.latitude}&UserId={CrossSceneInfoStatic.CurrentUser}"); 
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        var result = JsonUtility.FromJson<Response<AnchorDTO>>(jsonResponse);

        anchorLocateCriteria.Identifiers = result.data.Select(anchor => anchor.identifier).ToArray();

        await CloudManager.StartSessionAsync();

        if (currentWatcher != null)
        {
            currentWatcher.Stop();
            currentWatcher = null;
        }
        currentWatcher = CreateWatcher();
        if (currentWatcher == null)
        {
            Debug.Log("Either cloudmanager or session is null, should not be here!");
        }

    }
    
    protected CloudSpatialAnchorWatcher CreateWatcher()
    {
        if ((CloudManager != null) && (CloudManager.Session != null) && anchorLocateCriteria.Identifiers.Length > 0)
        {
            return CloudManager.Session.CreateWatcher(anchorLocateCriteria);
        }

        return null;
    }

    public virtual async Task SaveCurrentObjectAnchorToCloudAsync()
    {
        // Get the cloud-native anchor behavior
        CloudNativeAnchor cna = SpawnedObject.GetComponent<CloudNativeAnchor>();
        feedbackBox.text = Texts.getSavingSpawnedObject();

        // If the cloud portion of the anchor hasn't been created yet, create it
        if (cna.CloudAnchor == null) { cna.NativeToCloud(); }

        // Get the cloud portion of the anchor
        CloudSpatialAnchor cloudAnchor = cna.CloudAnchor;
        
        //cloudAnchor.Expiration = DateTimeOffset.Now.AddDays(30);

        String test = CloudManager.SessionStatus == null ? "null" : "not null";
        //feedbackBox.text = $"Cloud manager is ready: {CloudManager.IsReadyForCreate} and status: {test}";

        progressBar.gameObject.transform.localScale = Vector3.one;
        while (!CloudManager.IsReadyForCreate)
        {
            await Task.Delay(330);
            float createProgress = CloudManager.SessionStatus.RecommendedForCreateProgress;
            feedbackBox.text = Texts.getMoveYourDevice();
            progressBar.value = createProgress;
        }
        progressBar.gameObject.transform.localScale = Vector3.zero;

        bool success = false;

        feedbackBox.text = Texts.getSaving();

        try
        {
            // Actually save
            await CloudManager.CreateAnchorAsync(cloudAnchor);

            // Store
            var currentCloudAnchor = cloudAnchor;

            // Success?
            success = currentCloudAnchor != null;

            if (success && !isErrorActive)
            {
                // Await override, which may perform additional tasks
                // such as storing the key in the AnchorExchanger
                feedbackBox.text = Texts.getSavedAnchor() + "\n" +
                                   Texts.getTakeAScreenshot();
                _savedAnchor = currentCloudAnchor;
                
                //POSTING ANCHOR
                string jsonString = JsonUtility.ToJson(new  AnchorDTO {identifier = currentCloudAnchor.Identifier, model = this.model, userId = this.currentUser.id, longitude = Input.location.lastData.longitude, 
                    latitude = Input.location.lastData.latitude, srid = 4326, visibility = this.visibility, size = SpawnedObject.transform.localScale.x, communityId = this.visibility, CreationDate = DateTime.Now});
                Debug.Log("JSON ANCHOR : " + jsonString);

                using (HttpClient client = new HttpClient())
                {
                    Debug.Log("TOKEN : " + CrossSceneInfoStatic.Token);
                    HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CrossSceneInfoStatic.Token);
                    print("MESSAGE : " + httpContent.ReadAsStringAsync().Result);
                    HttpResponseMessage httpResponseMessage =
                        client.PostAsync($"{ApiURL}/api/AnchorsAPI", httpContent).Result;
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        HttpContent content = httpResponseMessage.Content;
                        string response = content.ReadAsStringAsync().Result;
                        //feedbackBox.text += $"\n API Response: {response}";

                        if (model.Equals("pannel"))
                        {
                            Board boardToSave = new Board
                                {title = CrossSceneInfoStatic.boardTitle, content = CrossSceneInfoStatic.boardContent};
                            boardToSave.anchorIndetifier = currentCloudAnchor.Identifier;
                            jsonString = JsonUtility.ToJson(boardToSave);

                            httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                            print("BOARD MESSAGE : " + httpContent.ReadAsStringAsync().Result);
                            httpResponseMessage = client.PostAsync($"{ApiURL}/api/PannelsAPI", httpContent).Result;
                        }
                        _uiHandler.SetScreenCaptureVisible(true);
                        await OnSaveCloudAnchorSuccessfulAsync();

                    }
                    else
                    {
                        feedbackBox.text = $"{Texts.getErrorResponse()}: {httpResponseMessage.StatusCode}";
                        Debug.LogError($"\n API ERROR Response: {httpResponseMessage.StatusCode}");
                        GetComponent<UIHandlerScript>().ToggleReload();
                    }
                }
            }
            else
            {
                feedbackBox.text = Texts.getFailedToSave();
                Debug.LogError("Failed to save, but no exception was thrown.");
                OnSaveCloudAnchorFailed(new Exception("Failed to save, but no exception was thrown."));
                GetComponent<UIHandlerScript>().ToggleReload();
            }
        }
        catch (Exception ex)
        {
            OnSaveCloudAnchorFailed(ex);
        }
    }

    protected virtual Task OnSaveCloudAnchorSuccessfulAsync()
    {
        // To be overridden.
        _uiHandler.SetButtonsActive(false);
        _uiHandler.SetPlusButtonActive(false);

        //Make spawned object interactable
        hasSaved = false;
        SpawnedObject = null;
        model = "";
        return Task.CompletedTask;
    }

    protected virtual void OnSaveCloudAnchorFailed(Exception exception)
    {
        // we will block the next step to show the exception message in the UI.
        isErrorActive = true;
        //feedbackBox.text = $"Error : {exception.ToString()}";
        hasSaved = false;

        //UnityDispatcher.InvokeOnAppThread(() => this.feedBack.text = string.Format("Error: {0}", exception.ToString()));
    }


    #endregion

    #region EventHandlers
    private void CloudManager_Error(object sender, SessionErrorEventArgs args)
    {
        throw new NotImplementedException();
    }

    private void CloudManagerOnLogDebug(object sender, OnLogDebugEventArgs args)
    {
        //throw new NotImplementedException();
    }

    private void CloudManagerOnAnchorLocated(object sender, AnchorLocatedEventArgs args)
    {
        if (args.Status == LocateAnchorStatus.Located)
        {
            //feedbackBox.text = $"Found anchor: {args.Anchor.Identifier}";
            var currentCloudAnchor = args.Anchor;
            UnityDispatcher.InvokeOnAppThread(() =>
            {
                Pose anchorPose = Pose.identity;
                
#if UNITY_ANDROID || UNITY_IOS
                anchorPose = currentCloudAnchor.GetPose();
#endif
                print("ANCHOR LOCATED : " + args.Anchor.Identifier);
                // HoloLens: The position will be set based on the unityARUserAnchor that was located.
                //SpawnOrMoveCurrentAnchoredObject(anchorPose.position, anchorPose.rotation);
                GameObject spawned = gameObject.GetComponent<ModelSpawnerScript>()
                    .SpawnAnchoredObject(args.Anchor.Identifier, anchorPose.position, anchorPose.rotation);
                EventTriggered?.Invoke(spawned);
            });
        }
    }
    
    private void CloudManager_LocateAnchorsCompleted(object sender, LocateAnchorsCompletedEventArgs args)
    {
        //throw new NotImplementedException();
    }

    private void CloudManager_SessionUpdated(object sender, SessionUpdatedEventArgs args)
    {
        Debug.Log(args.Status);
        //throw new NotImplementedException();
    }

    private void ReferencePointCreator_OnObjectPlacement(Transform transform)
    {
        if (string.IsNullOrEmpty(model)) return;
        if (hasSaved) return;

        if (SpawnedObject != null)
        {
            Destroy(SpawnedObject);
        }
        
        
        SpawnedObject = gameObject.GetComponent<ModelSpawnerScript>().SpawnNewObject(this.model, transform.position, transform.rotation);
        
        GetComponent<UIHandlerScript>().SaveMode(model);
        
    }
    #endregion // EventHandlers

    public void DestroyModel()
    {
        if (string.IsNullOrEmpty(model) || SpawnedObject == null) return;
        GetComponent<UIHandlerScript>().SetButtonsActive(false);
        model = "";
        Destroy(SpawnedObject);
        feedbackBox.text = Texts.getSelectAModelToSpawn();
    }
    
    IEnumerator StartLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start(10f, 0.05f);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("LOCATION: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

    }
}
