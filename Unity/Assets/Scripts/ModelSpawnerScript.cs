using System;
using System.IO;
using System.Net;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;

public class ModelSpawnerScript : MonoBehaviour
{
    [SerializeField] private  GameObject businessCardPrefab = null;
    [SerializeField] private  GameObject boardPrefab = null;
    [SerializeField] private  GameObject imagePrefab = null;
    [SerializeField] private  GameObject communityPlaceholder = null;
    
    public GameObject SpawnAnchoredObject(string AnchorId, Vector3 position, Quaternion rotation)
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Resources.Load<HIWConfig>("HIWConfig").apiBaseUrl + "/api/AnchorsAPI/" + AnchorId);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        AnchorDTO anchorFound = JsonUtility.FromJson<AnchorDTO>(jsonResponse);
        Debug.Log("JSON RESPONSE : " + jsonResponse);
        Debug.Log("JSON RESPONSE USER : " + anchorFound.userId);
        
        GameObject spawnedObject =  Instantiate(GetModelFromModelID(anchorFound.model), position, rotation);
        if (anchorFound.model.Equals("visit-card"))
        {
            anchorFound.user = FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + anchorFound.userId);
            Debug.Log("visit card user : " + anchorFound.user.nickName);
            spawnedObject.GetComponent<BusinessCardScript>().UpdateInfos(
                anchorFound.user.nickName ?? "",
                anchorFound.user.mission ?? "",
                anchorFound.user.email ?? "",
                anchorFound.user.phoneNumber ?? "",
                anchorFound.user.socialMedia ?? "");
        }
        else if (anchorFound.model.Equals("pannel"))
        {
            Board boardToLoad = FileAndNetworkUtils.getObjectFromApi<Board>("/api/PannelsAPI/" + anchorFound.identifier);
            Debug.Log("BOARD TITLE : " + boardToLoad.title);
            spawnedObject.GetComponent<BoardScript>().SetTitle(boardToLoad.title);
            spawnedObject.GetComponent<BoardScript>().SetContent(boardToLoad.content);
        }

        spawnedObject.transform.localScale = new Vector3(anchorFound.size, anchorFound.size, anchorFound.size);
        spawnedObject.AddComponent<AnchorInterraction>();
        spawnedObject.GetComponent<AnchorInterraction>().SetAnchorId(anchorFound.identifier);

        return spawnedObject;
    }
    
    public GameObject SpawnNewObject(string model, Vector3 position, Quaternion rotation)
    {
        GameObject newGameObject;
        newGameObject = Instantiate(GetModelFromModelID(model), position, rotation);

       
        
        switch (model)
        {
            case "visit-card":
            {
                //User currentUser = GameObject.Find("AzureSpatialAnchors").GetComponent<AzureSpatialTest>().currentUser;
                User currentUser = FileAndNetworkUtils.GetCurrentUser();
                print("PROFILE");
                print(currentUser.id ?? "");
                print(currentUser.nickName ?? "");
                print(currentUser.mission ?? "");
                print(currentUser.email ?? "");
                print(currentUser.phoneNumber ?? "");
                print(currentUser.socialMedia ?? "");
                newGameObject.GetComponent<BusinessCardScript>().UpdateInfos(
                    currentUser.nickName ?? "",
                    currentUser.mission ?? "",
                    currentUser.email ?? "",
                    currentUser.phoneNumber ?? "",
                    currentUser.socialMedia ?? "");
                break;
            }
            case "pannel":
            {
                BoardScript bs = newGameObject.GetComponent<BoardScript>();
                bs.SetTitle(CrossSceneInfoStatic.boardTitle);
                bs.SetContent(CrossSceneInfoStatic.boardContent);
                break;
            }
            case "image":
                newGameObject.GetComponent<ImageScript>().SetPicture(CrossSceneInfoStatic.ImageUrl);
                break;
        }
        //GET INFO BASED ON USERID
        newGameObject.AddComponent<CloudNativeAnchor>();
        //newGameObject.AddComponent<AnchorInterraction>();
        return newGameObject;
    }
    
    public GameObject GetModelFromModelID(string modelID)
    {
        //TO DO : get model from api
        Debug.Log("MODEL ID : " + modelID);
        Debug.Log(modelID);
        switch (modelID)
        {
            case "visit-card":
                return businessCardPrefab;
            case "pannel":
                return boardPrefab;
            case "image":
                return imagePrefab;
            /*case "community":
                int communityId = gameObject.GetComponent<AzureSpatialTest>().currentUser.communityId;
                string community = FileAndNetworkUtils.getObjectFromApi<Community>("/api/CommunitiesAPI/" + communityId).name;
                Debug.Log("Community : " + community);
                if (community.Equals("none")) return null;
                GameObject modelToReturn;
                modelToReturn = Resources.Load<GameObject>("Prefabs/Models/Communities/" + community);
                return modelToReturn != null ? modelToReturn : communityPlaceholder;*/
            default:
                return Resources.Load<GameObject>("Prefabs/Models/Others/" + modelID);
        }
    }
}
