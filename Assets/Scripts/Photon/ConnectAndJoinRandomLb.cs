using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectAndJoinRandomLb : MonoBehaviour,IConnectionCallbacks,IMatchmakingCallbacks,ILobbyCallbacks
{
    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private Button _createRoom;
    [SerializeField] private Button _joinRoom;
    [SerializeField] private ListItem _itemPrefab;
    [SerializeField] private Transform _root;
    [SerializeField] private TMP_InputField _nameOfCreatedRoom;

    private string _roomName;


    private LoadBalancingClient _lbc;
    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();

    public const string GAME_MODE_KEY = "GM";
    private const string AI_MODE_KEY = "AI";

    private const string MAP_PROP_KEY = "C0";
    private const string GOLD_PROP_KEY = "C1";

    private TypedLobby _sqlLobby = new TypedLobby("CustomAqlLobby", LobbyType.SqlLobby);
    private TypedLobby _defaultLobby = new TypedLobby("DefaultLobby", LobbyType.Default);

    private void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);
        _lbc.ConnectUsingSettings(_serverSettings.AppSettings);
        _createRoom.onClick.AddListener(() => CreateRoom());
        _joinRoom.onClick.AddListener(() => JoinRoomWithMyFriend(_nameOfCreatedRoom.text));

    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);
    }

    private void Update()
    {
        if (_lbc == null)
            return;
        _lbc.Service();
    }

    public void OnConnected()
    {
        
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        _lbc.OpJoinLobby(_defaultLobby);
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel(1);
        if (_lbc.CurrentRoom.MaxPlayers == 10)
        {
            _lbc.CurrentRoom.IsOpen = false;
        }
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
       
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        _cachedRoomList.Clear();
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        
    }

    public void OnJoinedRoom()
    {
        //var myFriend = _lbc.CurrentRoom.Players.Values.First().UserId;
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
       
    }

    public void OnLeftRoom()
    {
        
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    
    }

    public void OnJoinedLobby()
    {
        _cachedRoomList.Clear();
       
    }

    public void OnLeftLobby()
    {
        _cachedRoomList.Clear();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            ListItem listItem = Instantiate(_itemPrefab, _root);
            if (listItem != null)
            {
                listItem.SetInfo(info);
            }
        }
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
       
    }

    private void CreateRoom()
    {
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            PublishUserId = true,
            CustomRoomPropertiesForLobby = new[] { MAP_PROP_KEY, GOLD_PROP_KEY },
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { MAP_PROP_KEY, "Map3" }, { GOLD_PROP_KEY, 400 } }
            
        };
        var enterRoomParams = new EnterRoomParams
        {
            RoomName = "MyTestRoom",
            RoomOptions = roomOptions,
            Lobby = _defaultLobby
        };
        _lbc.OpCreateRoom(enterRoomParams);
    }

    private void JoinRoomWithMyFriend(string nameOfRoom)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = nameOfRoom;
        enterRoomParams.RoomOptions = roomOptions;
        _lbc.OpJoinOrCreateRoom(enterRoomParams);
    }
}
