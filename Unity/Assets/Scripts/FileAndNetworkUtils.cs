using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = System.Object;

public class FileAndNetworkUtils : MonoBehaviour
{
    private static string userID;
    
    public static User currentUser { get; set; }
    

    public static User GetCurrentUser()
    {
        //GETTING USER ID
        string path = GetPath();
        string _filePath= path + "/user.txt";

        try
        {
            FileStream _file = new System.IO.FileStream(_filePath,FileMode.OpenOrCreate);
            if (_file.Length < 1)
            {
                return null;
            }

            byte[] bytesRead = new byte[_file.Length];
            _file.Read(bytesRead, 0, bytesRead.Length);
            userID = Encoding.ASCII.GetString(bytesRead);
            _file.Close();
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError("ERROR :" + e);
            return null;
        }
        
        try
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Resources.Load<HIWConfig>("HIWConfig").apiBaseUrl + "/api/UsersAPI/" + userID);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            currentUser = JsonUtility.FromJson<User>(jsonResponse);
        }
        catch (WebException e)
        {
            Debug.LogError("ERROR :" + e);
            return null;
        }
        
        print("CURRENT USER : " + currentUser.id);
        
        return currentUser;
    }

    public static void DeleteCurrentUser()
    {
        SaveElementToFile("user", "");
    }

    public static void SaveElementToFile(string element, string value)
    {
        //GETTING USER ID
        string path = GetPath();
        string _filePath= path + $"/{element}.txt";
        
        FileStream _file = new System.IO.FileStream(_filePath,FileMode.Create);
        byte[] bytesCreate = Encoding.ASCII.GetBytes(value);
        _file.Write(bytesCreate, 0, bytesCreate.Length); //Write
        _file.Close();
    }
    
    public static string GetElementFromFile(string element)
    {
        //GETTING USER ID
        string path = GetPath();
        string _filePath= path + $"/{element}.txt";
        
        try
        {
            FileStream _file = new System.IO.FileStream(_filePath,FileMode.OpenOrCreate);
            if (_file.Length < 1)
            {
                return null;
            }

            byte[] bytesRead = new byte[_file.Length];
            _file.Read(bytesRead, 0, bytesRead.Length);
            string value = Encoding.ASCII.GetString(bytesRead);
            _file.Close();
            return value;
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError("ERROR :" + e);
            return null;
        }
    }

    public static string GetPath()
    {
        string path;
        
/*#if UNITY_ANDROID && !UNITY_EDITOR
        
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                path = jo.Call<AndroidJavaObject>("getDir", "", 0).Call<string>("getAbsolutePath");
            }
        }
        path = path.Replace("app_", "");
        path += "files";
#else*/
        path = Path.GetFullPath(Application.persistentDataPath);
//#endif
        return path;
    }

    public static T getObjectFromApi<T>(string endpoint, bool token = false)
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Resources.Load<HIWConfig>("HIWConfig").apiBaseUrl + endpoint);
        request.Headers.Add("Authorization", "Bearer " + CrossSceneInfoStatic.Token);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        return JsonUtility.FromJson<T>(jsonResponse);
    }

    public static HttpStatusCode postObjectToApi(string endpoint, object objectToPost)
    {
        string jsonString = JsonUtility.ToJson(objectToPost);
        Debug.Log("POST JSON : " + jsonString);

        using (HttpClient client = new HttpClient())
        {
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            print("POST MESSAGE : " + httpContent.ReadAsStringAsync().Result);
            HttpResponseMessage httpResponseMessage = client.PostAsync(Resources.Load<HIWConfig>("HIWConfig").apiBaseUrl + endpoint, httpContent).Result;
            return httpResponseMessage.StatusCode;
        }
    }
    
    public static Texture2D DownloadImage(string MediaUrl)
    {   
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            return null;
        }

        Stopwatch sw = new Stopwatch();
        while (!request.downloadHandler.isDone)
        {
            if (sw.ElapsedMilliseconds > 5000) return null;
        }
        return ((DownloadHandlerTexture) request.downloadHandler).texture;
    }
    
    
}
