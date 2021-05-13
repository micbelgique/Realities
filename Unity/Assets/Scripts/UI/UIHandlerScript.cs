using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIHandlerScript : MonoBehaviour
{
    public static GameObject[] otherObjects;
    private SnapshotCamera snapCam;
    private string userCommunity;
    public Board board;
    
    [SerializeField] private Button RotateLeftButton;
    [SerializeField] private Button RotateRightButton;
    [SerializeField] private Button ScaleUpButton;
    [SerializeField] private Button ScaleDownButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button CancelButton;
    [SerializeField] private Button HideButton;
    [SerializeField] private Button ReloadButton;
    [SerializeField] private Button TakeScreenshotButton;
    [SerializeField] private Button SkipScreenshotButton;
    [SerializeField] private ScrollView CustomItemsScrollView;
    [SerializeField] private Camera snapshotCam = null;
    [SerializeField] private GameObject BoardWindow;
    [SerializeField] private AzureSpatialTest AST;
    [SerializeField] private Text feedbackBox;
    [SerializeField] private Text appTitleText;
    [SerializeField] private Text switchText;
    [SerializeField] private GameObject screenCapturePanel;
    [SerializeField] private GameObject ItemStoreWebview;
    [SerializeField] private GameObject LoadingPanel;
    public TMP_InputField BoardTitle, BoardContent, BoardPictureUrl;
    


    private void Awake()
    {
        CrossSceneInfoStatic.CurrentScene = "AR";
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                Texts.language = Texts.Language.fr;
                break;
            case SystemLanguage.English:
            default:
                Texts.language = Texts.Language.en;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetButtonsActive(false);
        SetScreenCaptureVisible(false);
        ReloadButton.gameObject.SetActive(false);

        SetLanguage();
        appTitleText.text = Texts.getPhysicalWorld();
        switchText.text = Texts.getSwitch();
        
        snapCam = snapshotCam.GetComponent<SnapshotCamera>();
        snapCam = SnapshotCamera.MakeSnapshotCamera();
        
        SetButtonsActive(false);

        if (CrossSceneInfoStatic.CurrentUser.Equals("visitor"))
        {
            HideButton.gameObject.SetActive(false);
            feedbackBox.text = Texts.getGuestMode();
        }
    }
    
    void Update () {
        // Code for OnMouseDown in the iPhone. Unquote to test.
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit)) {
                    hit.transform.gameObject.GetComponent<AnchorInterraction>().OnItemClicked();
                }
            }
        }
    }

    public void SetLanguage()
    {
        feedbackBox.text = Texts.getSelectAModelToSpawn();
        saveButton.GetComponentInChildren<Text>().text = Texts.getSave();
        ReloadButton.GetComponentInChildren<Text>().text = Texts.getRefreshPage();
        TakeScreenshotButton.GetComponentInChildren<Text>().text = Texts.getTakeScreenshot();
        SkipScreenshotButton.GetComponentInChildren<Text>().text = Texts.getSkip();
    }
    
    public void onPointerDownRotateLeftButton()
    {
        AST.isRotatingLeft = true;
    }
    public void onPointerUpRotateLeftButton()
    {
        AST.isRotatingLeft = false;
    }
    public void onPointerDownRotateRightButton()
    {
        AST.isRotatingRight = true;
    }
    public void onPointerUpRotateRightButton()
    {
        AST.isRotatingRight = false;
    }
    public void onPointerDownScaleUpButton()
    {
        AST.isScalingUp = true;
    }
    public void onPointerUpScaleUpButton()
    {
        AST.isScalingUp = false;
    }
    public void onPointerDownScaleDownButton()
    {
        AST.isScalingDown = true;
    }
    public void onPointerUpScaleDownButton()
    {
        AST.isScalingDown = false;
    }
    

    public void StartLoading()
    {
        LoadingPanel.transform.localScale = new Vector3(1,1,1);
    }

    public void StopLoading()
    {
        LoadingPanel.transform.localScale = new Vector3(0,0,0);
    }
    

    public void ToggleItemStore()
    {
        ItemStoreWebview.GetComponent<UniWebView>().ReferenceRectTransform = ItemStoreWebview.GetComponent<RectTransform>();
        GameObject.Find("ItemStoreWebview").gameObject.GetComponent<WebViewHandler>().LoadNewPage("/shop");
        StartCoroutine(UpdateStoreWebview());
    }
    
    IEnumerator UpdateStoreWebview() 
    {
        while(!ItemStoreWebview.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default") || ItemStoreWebview.GetComponent<Animator>().IsInTransition(0)) 
        {
            ItemStoreWebview.GetComponent<UniWebView>().ReferenceRectTransform = ItemStoreWebview.GetComponent<RectTransform>();
            yield return null;
        }
        ItemStoreWebview.GetComponent<UniWebView>().ReferenceRectTransform = ItemStoreWebview.GetComponent<RectTransform>();
    }
    
    public async void OnSaveButtonClicked()
    {
        SetButtonsActive(false);
        HideButton.gameObject.SetActive(false);
        AST.hasSaved = true;
        await AST.SaveCurrentObjectAnchorToCloudAsync();
    }
    
    private Sprite GetSpriteFromModelID(string modelID, float scale)
    {
        snapCam.defaultScale = new Vector3(scale, scale, scale);
        Texture2D buttonImage = snapCam.TakePrefabSnapshot(gameObject.GetComponent<ModelSpawnerScript>().GetModelFromModelID(modelID), new Color(0, 0.475f, 0.839f));
        return Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());
    }
    
    private Sprite GetSpriteFromModel(GameObject model, float scale)
    {
        snapCam.defaultScale = new Vector3(scale, scale, scale);
        Texture2D buttonImage = snapCam.TakePrefabSnapshot(model, new Color(0, 0.475f, 0.839f));
        return Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());
    }

    public void OnProfileButtonClicked()
    {
        StartLoading();
        //StartCoroutine("SceneSwitch");
        CrossSceneInfoStatic.CurrentScene = "Profile";
        SceneManager.LoadScene("WebView");
    }

    public void SetScreenCaptureVisible(bool visible)
    {
        screenCapturePanel.transform.localScale = visible ? Vector3.one : Vector3.zero;
    }

    public void SetButtonsActive(bool active, bool resize = true)
    {
        if (!active) resize = false;
        saveButton.gameObject.SetActive(active);
        RotateLeftButton.gameObject.SetActive(active);
        RotateRightButton.gameObject.SetActive(active);
        ScaleUpButton.gameObject.SetActive(resize);
        ScaleDownButton.gameObject.SetActive(resize);
        CancelButton.gameObject.SetActive(active);
        HideButton.gameObject.SetActive(!active);
    }
    
    public void SetPlusButtonActive(bool active)
    {
        HideButton.gameObject.SetActive(active);
    }
        
    IEnumerator SceneSwitch()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Profile", LoadSceneMode.Additive);
        yield return load;
        SceneManager.UnloadSceneAsync("AzureSpatialAnchorsBasicDemo");
    }

    public void SaveMode(string model)
    {
        feedbackBox.text = Texts.getClickSave();
        if(model.Equals("visit-card"))
            GetComponent<UIHandlerScript>().SetButtonsActive(true, false);
        else
            GetComponent<UIHandlerScript>().SetButtonsActive(true);
    }

    public void ToggleReload()
    {
        ReloadButton.gameObject.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
