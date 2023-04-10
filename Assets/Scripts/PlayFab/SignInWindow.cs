using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();
        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _userName,
            Password = _password
        }, result =>
        {
           Debug.Log($"Success: {_userName}");
           EnterInGameScene();
        }, error =>
        {
            Debug.Log($"Fail: {error.ErrorMessage}");
        });
    }    
}
