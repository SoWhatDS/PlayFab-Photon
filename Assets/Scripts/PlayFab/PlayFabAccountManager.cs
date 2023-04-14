
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
using UnityEngine.TextCore.Text;

public class PlayFabAccountManager : MonoBehaviour
{
    //[SerializeField] private TMP_Text _titleWeapon;
    //[SerializeField] private GameObject _swordCanvas;
    //[SerializeField] private GameObject _bowCanvas;
    //[SerializeField] private GameObject _itemView;

    //[SerializeField] private Button _showSwords;
    //[SerializeField] private Button _showBows;

    //private List<string> _swords;
    //private List<string> _bows;
    //private Dictionary<string, GameObject> _createdItems;

    private const string _swordType = "Sword";
    private const string _bowType = "Bow";

    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private GameObject _newCharacterCreatePanel;
    [SerializeField] private Button _createCharacterButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] List<SlotCharacterWidget> _slots;
    [SerializeField] private GameObject _rootSlot;
    [SerializeField] private SlotCharacterWidget _slot;
    

    private string _characterName;


    private void Start()
    {
        /*_swords = new List<string>();
        _bows = new List<string>();
        _createdItems = new Dictionary<string, GameObject>();

        _showSwords.onClick.AddListener(() => ShowSwordsInLobby(_swords,_swordCanvas.transform));
        _showBows.onClick.AddListener(() => ShowBowsInLobby(_bows,_bowCanvas.transform));

        DeactivateAllLobby();*/

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),OnGetAccount,OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);
        PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), OnGetRandomResultTables, OnError);

        GetCharacters();

        SubscribeCreateButtonSlot();
        _inputField.onValueChanged.AddListener(OnNameChanged);
        _createCharacterButton.onClick.AddListener(CreateCharacter);
        CreateNewSlotCharacter();
    }

    private void SubscribeCreateButtonSlot()
    {
        foreach (var slot in _slots)
        {
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);
        }
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_warrior_token"
        }, result =>
        {
            CreateNewSlotCharacter();
            UpdateCharacterStatistics(result.CharacterId);
        }, OnError);
    }

    private void CreateNewSlotCharacter()
    {
        var go = Instantiate(_slot, _rootSlot.transform);
        go.ShowEmptySlot();
        go.GetComponentInChildren<Button>().onClick.AddListener(OpenCreateNewCharacter);
        _slots.Add(go);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>
            {
                {"Damage", 10 },
                { "Health",100},
                { "Exp",1}
            }

            }, result =>
            {
                Debug.Log("Complete character statistics");
                CloseCreateNewCharacter();
                GetCharacters();
            }, OnError);
    }

    private void CloseCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(false);
    }

    private void OnNameChanged(string changedName)
    {
        _characterName = changedName;
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(true);
    }

    private void GetCharacters()
    {   
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest
        {
  
        }, result =>
        {
            Debug.Log($"Character count: {result.Characters.Count}");
            ShowCharacterInSlot(result.Characters);
        }, OnError);
    }

    private void ShowCharacterInSlot(List<CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _slots)
            {
                slot.ShowEmptySlot();
            }
        }
        else if (characters.Count > 0 && characters.Count < 10)
        {
            ShowInSlot(characters);       
        }
        else
        {
            CreateNewSlotCharacter();
        }
    }

    private void ShowInSlot(List<CharacterResult> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = characters[i].CharacterId
               
            }, result =>
            {
                //var level = result.CharacterStatistics["Level"].ToString();
                //var gold = result.CharacterStatistics["Gold"].ToString();
                var damage = result.CharacterStatistics["Damage"].ToString();
                var health = result.CharacterStatistics["Health"].ToString();
                var exp = result.CharacterStatistics["Exp"].ToString();
                _slots[0].ShowInfoCharacterInfo(damage, health, exp);
                //_slots[1].ShowInfoCharacterInfo(characters.First().CharacterName, gold, level);

            }, OnError); ;
        }
    }

    private void OnGetRandomResultTables(PlayFab.ServerModels.GetRandomResultTablesResult result)
    {
        //получаем RandomTables;
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        //PlayFabInfo(result);
        _titleLabel.text = $"Playfab id: {result.AccountInfo.PlayFabId}";
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
       // InitListsWithWeapon(result.Catalog);
    }

    /*private void InitListsWithWeapon(List<CatalogItem> catalog)
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
    }*/

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

   /* private void ShowSwordsInLobby(List<string> swords,Transform root)
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
    }*/
}
