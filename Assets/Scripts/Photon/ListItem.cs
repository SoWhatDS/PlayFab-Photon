using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;

public class ListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _textName;
    [SerializeField] private TMP_Text _textPlayerCount;

    public void SetInfo(RoomInfo info)
    {
        _textName.text = info.Name;
        _textPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }
}
