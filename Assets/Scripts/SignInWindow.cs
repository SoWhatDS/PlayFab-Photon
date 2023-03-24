using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Canvas _enterLogin;
    [SerializeField] private Canvas _enterNewPassword;
    [SerializeField] private Canvas _signInWindow;
    [SerializeField] private InputField _login;
    [SerializeField] private InputField _newPassword;

    private int _counter;
    private const int _maxTry = 3;

    private void Start()
    {
        _login.onValueChanged.AddListener(UpdateLogin);
        _enterLogin.gameObject.SetActive(false);
        _newPassword.gameObject.SetActive(false);
        _signInButton.onClick.AddListener(SignIn);
    }

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
            _counter++;
            Debug.Log($"Fail: {_counter}");
            if (_counter == _maxTry)
            {
                UpdatePasswordForNew();
            }
            Debug.Log($"Fail: {error.ErrorMessage}");
        });
    }

    private void UpdatePasswordForNew()
    {
        _enterLogin.gameObject.SetActive(true);
        if (_userName == _login.text)
        {
            _enterLogin.gameObject.SetActive(false);
            _enterNewPassword.gameObject.SetActive(true);
            _newPassword.onValueChanged.AddListener(UpdatePassword);
            _enterNewPassword.gameObject.SetActive(false);
        }
    }

    private void UpdatePassword(string password)
    {
        _password = _newPassword.text;
    }

    private void UpdateLogin(string login)
    {
        _login.text = login;
    }
    
}
