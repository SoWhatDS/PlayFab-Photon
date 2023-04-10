
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _disconnect;
    [SerializeField] private Button _connect;
    [SerializeField] private TMP_Text _label;

    

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        _connect.onClick.AddListener(() => Connect());
        _disconnect.onClick.AddListener(() => Disconnect());
    }

    private void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
        _label.color = Color.green;
        _label.text = "You are connected to server";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.CreateRoom("NewName");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
        _label.color = Color.red;
        _label.text = "You are disconnected";
        Debug.Log("You are disconnect");
    } 
}
