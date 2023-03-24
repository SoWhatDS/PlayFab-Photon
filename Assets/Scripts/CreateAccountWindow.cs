
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _mailField;
    [SerializeField] private Button _createAccountButton;
    [SerializeField] private TMP_Text _textAccountInfo;

    private string _mail;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();
        _mailField.onValueChanged.AddListener(UpdateMail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _userName,
            Email = _mail,
            Password = _password
        }, result =>
        {
            Debug.Log($"Success:  {_userName}");
            PlayFabAccountInfo();
            //EnterInGameScene();
        }, error =>
        {
            Debug.Log($"Fail: {error.ErrorMessage}");
        });
    }

    private void UpdateMail(string mail)
    {
        _mail = mail;
    }

    private void PlayFabAccountInfo()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);

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

        _textAccountInfo.text = $"Playfab id: {playfabId} \n// Playfab Name: {playfabName} " +
            $"\n// Playfab created time: {playfabCreatedTime} \n\\ userName: {_userName} /n\\ userPassword {_password}";
    }
}
