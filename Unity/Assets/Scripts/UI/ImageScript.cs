using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageScript : MonoBehaviour
{
    [SerializeField] private GameObject picturePlane;
    
    public void SetPicture(string imageUrl)
    {
        StartCoroutine(DownloadImage(imageUrl));
    }
    
    IEnumerator DownloadImage(string MediaUrl)
    {
        Debug.Log("MEDIA : " + MediaUrl);
        Texture2D texture;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError) 
            Debug.Log(request.error);
        else
        {
            texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Material material = new Material(Shader.Find("Diffuse"));
            material.mainTexture = texture;
            picturePlane.GetComponent<Renderer>().material = material;
        }
    }
}
