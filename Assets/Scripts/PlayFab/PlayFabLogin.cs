
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayFabLogin : MonoBehaviour
{
    //[SerializeField] private Button _logIn;
    //[SerializeField] private TMP_Text _labelPlayFab;
    //[SerializeField] private Button _connect;
    //[SerializeField] private Button _disconnect;
    //[SerializeField] private TMP_Text _labelPhoton;

    [SerializeField] private GameObject _playerPrefab;

    private const string AuthGuidKey = "auth_guid_key";


    private void Start()
    {
        //_logIn.onClick.AddListener(() => LogInToServer());
        //_connect.onClick.AddListener(() => ConnectToPhoton());
        //_disconnect.onClick.AddListener(() => DisconnectFromPhoton());
        //SetActiveButtons();
        LogInToServer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakePurchase();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetInventory();
        }
    }

    private void LogInToServer()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "2A7EA";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey,Guid.NewGuid().ToString());
        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                OnLoginSuccess(result);
            }, OnLoginError) ;
    }

    private void OnLoginSuccess(LoginResult result)
    {

       // _labelPlayFab.color = Color.green;
        //_labelPlayFab.text = "You are connect to Server";
        SetUserData(result.PlayFabId);
        //MakePurchase();
        GetInventory();
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result => ShowInventory(result.Inventory), OnLoginError);
    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        var firstItem = inventory.First();
        Debug.Log(inventory.Count);
        Debug.Log($"{ firstItem.ItemId}");
        ConsumePotion(firstItem.ItemInstanceId);
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId
        },
        result =>
        {
            Debug.Log("Complete ConsumeItem");
       
        }, OnLoginError);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "Main",
            ItemId = "health_poition",
            Price = 3,
            VirtualCurrency = "SC"
        },
        result =>
        {
            for (int i = 0; i < result.Items.Count; i++)
            {
                Debug.Log(result.Items[i]);
            }
            Debug.Log("Complete purchase item");
        }, OnLoginError);
    }

    private void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Time_receive_daily_reward",DateTime.UtcNow.ToString()}
            }
        },
        result =>
        {
            Debug.Log("SetUserData");
            GetUserData(playFabId, "Time_receive_daily_reward");
        },
        OnLoginError);
    }

    private void GetUserData(string playFabId, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playFabId
        },
        result =>
        {
            if (result.Data.ContainsKey(keyData))
            {
                Debug.Log($"{keyData}: {result.Data[keyData].Value}");
            }
        }, OnLoginError);
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        //_labelPlayFab.color = Color.red;
        //_labelPlayFab.text = "Error!!!";
        Debug.Log(errorMessage);
    }

    private void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
       // _labelPhoton.color = Color.green;
        //_labelPhoton.text = "You are connected to server";
        SetActiveButtons();
    }

    private void DisconnectFromPhoton()
    {
        PhotonNetwork.Disconnect();
       // _labelPhoton.color = Color.red;
       // _labelPhoton.text = "You are disconnected";
        Debug.Log("You are disconnect");
        SetActiveButtons();
    }

    private void SetActiveButtons()
    {
        if (PhotonNetwork.IsConnected)
        {
           // _connect.gameObject.SetActive(false);
            //_disconnect.gameObject.SetActive(true);
        }
        else
        {
           // _connect.gameObject.SetActive(true);
           // _disconnect.gameObject.SetActive(false);
        }
    }
}
