using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Android;
using Button = UnityEngine.UI.Button;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [Header("Fields")] 
    [Header("Register")]
    [SerializeField] public TMP_InputField log_email;
    [SerializeField] public TMP_InputField log_password;
    
    [Header("Register")]
    [SerializeField] public TMP_InputField nickname;
    [SerializeField] public TMP_InputField email;
    [SerializeField] public TMP_InputField phone;
    [SerializeField] public TMP_InputField function;
    [SerializeField] public TMP_InputField mission;
    [SerializeField] public TMP_InputField password;

    [Header("Buttons")] 
    [SerializeField] public Button validateButton;

    [Header("Config")] 
    [SerializeField] public HIWConfig config;
    
    [Header("Errors")] 
    [SerializeField] public TMP_Text loginError;
    [SerializeField] public TMP_Text registerError;
    
    void Start()
    {
        Permission.RequestUserPermission(Permission.FineLocation);
        validateButton.interactable = false;
        if(FileAndNetworkUtils.GetCurrentUser() != null)
            SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
    }

    private void FixedUpdate()
    {
        if (!String.IsNullOrEmpty(nickname.text)
            && !String.IsNullOrEmpty(email.text))
            validateButton.interactable = true;
        else
            validateButton.interactable = false;
    }

    public async void OnButtonSelected()
    {
        clearErrors();
        
        Debug.Log($"nickname = [{nickname.text}]");
        Debug.Log($"email = [{email.text}]");
        Debug.Log($"phone = [{phone.text}]");
        Debug.Log($"mission = [{mission.text}]");
        Debug.Log($"function = [{function.text}]");
        Debug.Log($"function = [{password.text}]");

        using (HttpClient client = new HttpClient())
        {
            User user = new User(
                this.nickname.text,
                this.email.text,
                this.phone.text,
                "social",
                this.function.text,
                this.mission.text,
                this.password.text);
            
            string jsonString = JsonUtility.ToJson(user);

            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            Debug.Log("MESSAGE : " + jsonString);
            HttpResponseMessage httpResponseMessage = client.PostAsync($"{config.apiBaseUrl}/api/UsersAPI", httpContent).Result;
            if(httpResponseMessage.IsSuccessStatusCode)
            {
                HttpContent content = httpResponseMessage.Content;
                string response = content.ReadAsStringAsync().Result;
                //todo: register to local storage + send to next page
                Debug.Log(response);
                User savedUser = JsonUtility.FromJson<User>(response);
                Debug.Log("USER TO SAVE : " + savedUser.id);
                FileAndNetworkUtils.SaveElementToFile("user", savedUser.id);
                SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
            }
            else
            {
                HttpContent content = httpResponseMessage.Content;
                string errorResponse = content.ReadAsStringAsync().Result;
                Debug.Log(errorResponse);
                ErrorResponse response = JsonUtility.FromJson<ErrorResponse>(errorResponse);
                registerError.text = response.message;
            }
        }
    }

    public void OnLoginButtonClicked()
    {
        clearErrors();
        ConnectionEntity entity = new ConnectionEntity();
        entity.email = log_email.text;
        entity.password = log_password.text;
        
        string jsonString = JsonUtility.ToJson(entity);
        using (HttpClient client = new HttpClient())
        {
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            Debug.Log("MESSAGE : " + jsonString);
            HttpResponseMessage httpResponseMessage =
                client.PostAsync($"{config.apiBaseUrl}/api/Authentication", httpContent).Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                HttpContent content = httpResponseMessage.Content;
                string response = content.ReadAsStringAsync().Result;
                //todo: register to local storage + send to next page
                Debug.Log(response);
                AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(response);
                Debug.Log(authResponse);
                //Debug.Log("USER TO SAVE : " + savedUser.id);
                //FileReaderScript.SaveUser(savedUser.id);
                //SceneManager.LoadScene("AzureSpatialTest", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
            }
            else
            {
                HttpContent content = httpResponseMessage.Content;
                string errorResponse = content.ReadAsStringAsync().Result;
                Debug.Log(errorResponse);
                
                loginError.text = "Error : email or password incorrect";
            }
        }

        
    }

    public void clearErrors()
    {
        loginError.text = "";
        registerError.text = "";
    }
    
}
