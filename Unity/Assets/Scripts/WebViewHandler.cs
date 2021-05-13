using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(UniWebView))]
public class WebViewHandler : MonoBehaviour
{
    private UniWebView webView;
    private HIWConfig config;
    void Start()
    {
        config = Resources.Load<HIWConfig>("HIWConfig");
        // Add a full-screen UniWebView component.
        webView = GetComponent<UniWebView>();
        webView.CleanCache();
        
        webView.OnPageStarted += (view, url) => {
            print("Loading started for url: " + url);
        };
        // Load a URL.
        switch (CrossSceneInfoStatic.CurrentScene)
        {
            case "Details":
                webView.Load(config.frontendUrl + $"/anchorDetail/{CrossSceneInfoStatic.TagForTagDetails}");
                break;
            case "AR" : 
                webView.Load(config.frontendUrl + "/shop");
                break;
            case "Profile":
            default:
                webView.Load(config.frontendUrl + "/profile");
                break;
        }

        
        webView.AddUrlScheme("unity-app");
        webView.OnMessageReceived += WebViewOnOnMessageReceived;

        webView.Show();
    }

    private void WebViewOnOnMessageReceived(UniWebView webview, UniWebViewMessage message)
    {
        print("SCHEME " + message.Scheme);
        print("PATH " + message.Path);
        print("MESSAGE RECEIVED : " + message.RawMessage);

        switch (message.Path)
        {
           case "Login":
               var token = message.Args["token"];
               var userId = message.Args["userId"];

               CrossSceneInfoStatic.CurrentUser = userId;
               FileAndNetworkUtils.SaveElementToFile("user", userId);
               FileAndNetworkUtils.SaveElementToFile("token", token);
               SceneManager.LoadScene("Scenes/AzureSpatialAnchorsBasicDemo");
               
               break;
           case "ModelSelected":
               var model = message.Args["model"];
               var visibility = message.Args["visibility"];
               var community = message.Args["community"];

               AzureSpatialTest AST = GameObject.Find("AzureSpatialAnchors").GetComponent<AzureSpatialTest>();
               
               /*CrossSceneInfoStatic.ModelSelected = model;
               CrossSceneInfoStatic.VisibilitySelected = int.Parse(visibility);
               CrossSceneInfoStatic.CommunitySelected = int.Parse(community);*/
               AST.model = model;
               AST.visibility = int.Parse(visibility);
               AST.community = int.Parse(community);
               switch (model)
               {
                   case "pannel":
                       CrossSceneInfoStatic.boardContent = message.Args["content"];
                       CrossSceneInfoStatic.boardTitle = message.Args["title"];
                       break;
                   case "image":
                       CrossSceneInfoStatic.ImageUrl = message.Args["imageUrl"];
                       break;
               }

               GameObject.Find("ItemStoreWebview").GetComponent<Animator>().SetTrigger("ToggleMenu");
               GameObject.Find("feedbackText").GetComponent<Text>().text = Texts.getTouchASurface();

               break;
           case "Logout":
               FileAndNetworkUtils.SaveElementToFile("user", "visitor");
               CrossSceneInfoStatic.CurrentUser = "visitor";
               break;
        }
    }

    public void LoadNewPage(string endpoint)
    {
        webView.Load(config.frontendUrl + endpoint);
    }
}
