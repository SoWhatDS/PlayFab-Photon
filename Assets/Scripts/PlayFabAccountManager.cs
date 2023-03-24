
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),OnGetAccount,OnError);
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

        _titleLabel.text = $"Playfab id: {playfabId} \n// Playfab Name: {playfabName} \n// Playfab created time: {playfabCreatedTime}";

    }
}
