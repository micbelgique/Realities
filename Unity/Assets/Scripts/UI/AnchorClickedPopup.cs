using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnchorClickedPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text AnchorText;
    [SerializeField] private TMP_Text UserText;
    [SerializeField] private TMP_Text Header;
    [SerializeField] private TMP_Text CancelText;
    [SerializeField] private TMP_Text DetailsText;

    private AnchorDTO anchor;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localScale = new Vector3(0, 0, 0);
        Header.text = Texts.getAnchorClicked();
        CancelText.text = Texts.getCancel();
        DetailsText.text = Texts.getDetails();
    }
    
    public void ShowPopup(string anchorId)
    {
        this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        anchor = FileAndNetworkUtils.getObjectFromApi<AnchorDTO>("/api/AnchorsAPI/" + anchorId);
        AnchorText.text = anchor.identifier;
        UserText.text = Texts.getPlacedBy() + " " + FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + anchor.userId).nickName;
    }

    public void ClosePopup()
    {
        this.gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
   
    
    public void OnDetailsButtonClicked()
    {
        CrossSceneInfoStatic.TagForTagDetails = anchor.identifier;
        CrossSceneInfoStatic.CurrentScene = "Details";
        SceneManager.LoadScene("WebView");
        ClosePopup();
    }
}
