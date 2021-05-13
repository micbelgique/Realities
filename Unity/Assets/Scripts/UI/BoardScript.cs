using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class BoardScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro Title, Content;
    public string AnchorId { get; set; }

    public void SetTitle(string title)
    {
        Title.text = title;
    }
    
    public void SetContent(string content)
    {
        Content.text = content;
    }

    
}