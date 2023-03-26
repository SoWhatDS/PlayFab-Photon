
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
            EnterInGameScene();
        }, error =>
        {
            Debug.Log($"Fail: {error.ErrorMessage}");
        });
    }

    private void UpdateMail(string mail)
    {
        _mail = mail;
    }
}
