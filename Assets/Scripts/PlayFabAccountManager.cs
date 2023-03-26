
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleWeapon;
    [SerializeField] private GameObject _swordCanvas;
    [SerializeField] private GameObject _bowCanvas;
    [SerializeField] private GameObject _itemView;

    [SerializeField] private Button _showSwords;
    [SerializeField] private Button _showBows;

    private List<string> _swords;
    private List<string> _bows;
    private Dictionary<string, GameObject> _createdItems;

    private const string _swordType = "Sword";
    private const string _bowType = "Bow";


    private void Start()
    {
        _swords = new List<string>();
        _bows = new List<string>();
        _createdItems = new Dictionary<string, GameObject>();

        _showSwords.onClick.AddListener(() => ShowSwordsInLobby(_swords,_swordCanvas.transform));
        _showBows.onClick.AddListener(() => ShowBowsInLobby(_bows,_bowCanvas.transform));

        DeactivateAllLobby();

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),OnGetAccount,OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);
        PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), OnGetRandomResultTables, OnError);
    }

    private void OnGetRandomResultTables(PlayFab.ServerModels.GetRandomResultTablesResult result)
    {
        //получаем RandomTables;
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        PlayFabInfo(result);
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.Log(errorMessage);
    }

    private void PlayFabInfo(GetAccountInfoResult result)
    {
        string playfabId = result.AccountInfo.PlayFabId;
        string playfabName = result.AccountInfo.Username;
        string playfabCreatedTime = result.AccountInfo.Created.ToString();

    }

    private void OnGetCatalog(GetCatalogItemsResult result)
    {
        Debug.Log("OnGetCatalogSuccess");
        InitListsWithWeapon(result.Catalog);
    }

    private void InitListsWithWeapon(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            if (item.ItemClass == _swordType)
            {
                _swords.Add(item.DisplayName);
            }
            else if (item.ItemClass == _bowType)
            {
                _bows.Add(item.DisplayName);
            }
        }
    }

    private void ShowSwords(List<string> swords)
    {
        foreach (var item in swords)
        {
            Debug.Log(item);
        }
    }

    private void ShowBows(List<string> bows)
    {
        foreach (var item in bows)
        {
            Debug.Log(item);
        }
    }

    private void ShowSwordsInLobby(List<string> swords,Transform root)
    {
        if (_swordCanvas.active)
        {
            return;
        }
        DeactivateAllLobby();
        SetActiveLobby(_swordCanvas);
        for (int i = 0; i < swords.Count; i++)
        {
            ShowWeapon(swords[i],root);
        }
        _titleWeapon.text = _swordType;
    }

    private void ShowBowsInLobby(List<string> bows,Transform root)
    {
        if (_bowCanvas.active)
        {
            return;
        }
        DeactivateAllLobby();
        SetActiveLobby(_bowCanvas);
        for (int i = 0; i < bows.Count; i++)
        {
            ShowWeapon(bows[i],root);
        }
        _titleWeapon.text = _bowType;
    }

    private void ShowWeapon(string weapon,Transform root)
    {
       if (_createdItems.ContainsKey(weapon))
       {
           return;
       }
       var go = Instantiate(_itemView, root.transform.position, Quaternion.identity);
        _createdItems.Add(weapon, go);
       go.transform.SetParent(root);
       var title = go.GetComponentInChildren<TMP_Text>();   
       title.text = weapon;
    }

    private void SetActiveLobby(GameObject panel)
    {
        panel.gameObject.SetActive(true);
    }

    private void DeactivateAllLobby()
    {
        _swordCanvas.SetActive(false);
        _bowCanvas.SetActive(false);
    }
}
