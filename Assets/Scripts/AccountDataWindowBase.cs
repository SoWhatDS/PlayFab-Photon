using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class AccountDataWindowBase : MonoBehaviour
{
    [SerializeField] private InputField _userNameField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private Slider _loadSlider;
    [SerializeField] private TMP_Text _textLoading;
    [SerializeField] private Canvas _loading;

    protected string _userName;
    protected string _password;

    private void Start()
    {
        SubscriptionsElementsUI();
        _loading.gameObject.SetActive(false);
    }

    protected virtual void SubscriptionsElementsUI()
    {
        _userNameField.onValueChanged.AddListener(UpdateUserName);
        _passwordField.onValueChanged.AddListener(UpdatePassword);
    }

    private void UpdateUserName(string userName)
    {
        _userName = userName;
    }

    private void UpdatePassword(string password)
    {
        _password = password;
    }

    protected void EnterInGameScene()
    {
        StartCoroutine(AsyncLoad());
    }

    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        while (!operation.isDone)
        {
            float progress = operation.progress/0.9f;
            _loading.gameObject.SetActive(true);
            _loadSlider.value = progress;
            _textLoading.text = "Loading ... " + string.Format("{0:0}%", progress * 100);
            Debug.Log(_textLoading.text);
            Debug.Log(_loadSlider.value);
            yield return null;
            _loading.gameObject.SetActive(false);
        }
    }
}
