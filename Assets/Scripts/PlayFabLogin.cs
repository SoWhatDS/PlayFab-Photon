
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private Button _logIn;
    [SerializeField] private TMP_Text _labelPlayFab;
    [SerializeField] private Button _connect;
    [SerializeField] private Button _disconnect;
    [SerializeField] private TMP_Text _labelPhoton;


    private void Start()
    {
        _logIn.onClick.AddListener(() => LogInToServer());
        _connect.onClick.AddListener(() => ConnectToPhoton());
        _disconnect.onClick.AddListener(() => DisconnectFromPhoton());
        SetActiveButtons();
    }

    private void LogInToServer()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "2A7EA";
        }
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Player 1",
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _labelPlayFab.color = Color.green;
        _labelPlayFab.text = "You are connect to Server";
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        _labelPlayFab.color = Color.red;
        _labelPlayFab.text = "Error!!!";
        Debug.Log(errorMessage);
    }

    private void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
        _labelPhoton.color = Color.green;
        _labelPhoton.text = "You are connected to server";
        SetActiveButtons();
    }

    private void DisconnectFromPhoton()
    {
        PhotonNetwork.Disconnect();
        _labelPhoton.color = Color.red;
        _labelPhoton.text = "You are disconnected";
        Debug.Log("You are disconnect");
        SetActiveButtons();
    }

    private void SetActiveButtons()
    {
        if (PhotonNetwork.IsConnected)
        {
            _connect.gameObject.SetActive(false);
            _disconnect.gameObject.SetActive(true);
        }
        else
        {
            _connect.gameObject.SetActive(true);
            _disconnect.gameObject.SetActive(false);
        }
    }
}
