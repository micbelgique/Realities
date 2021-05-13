using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class PermissionScript : MonoBehaviour
{
    private void Awake()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
        SceneManager.LoadScene(string.IsNullOrEmpty(FileAndNetworkUtils.GetElementFromFile("user"))
            ? "WebView"
            : "AzureSpatialAnchorsBasicDemo");
    }
}
