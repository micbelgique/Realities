using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnchorInterraction : MonoBehaviour
{
    private GameObject isw;
    
    public string anchorId;
    
    public void SetAnchorId(string anchorId)
    {
        this.anchorId = anchorId;
    }

    public void OnItemClicked()
    {
        if (IsPointerOverUIObject()) return;
        Debug.Log("TOUCHED ITEM " + anchorId);
        isw = GameObject.Find("ItemStoreWebview").gameObject;
        isw.GetComponent<WebViewHandler>().LoadNewPage("/anchorPreview/" + anchorId);
        isw.GetComponent<Animator>().SetTrigger("ShowMenu");
        isw.GetComponent<UniWebView>().ReferenceRectTransform = isw.GetComponent<RectTransform>();
        StartCoroutine(UpdateStoreWebview());
    }
    
    IEnumerator UpdateStoreWebview() 
    {
        while(!isw.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default") || isw.GetComponent<Animator>().IsInTransition(0)) 
        {
            isw.GetComponent<UniWebView>().ReferenceRectTransform = isw.GetComponent<RectTransform>();
            yield return null;
        }
        isw.GetComponent<UniWebView>().ReferenceRectTransform = isw.GetComponent<RectTransform>();
    }
    
    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}