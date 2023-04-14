using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _emtySlot;
    [SerializeField] private GameObject _infoCharacterSlot;
    [SerializeField] private TMP_Text _textLabel1;
    [SerializeField] private TMP_Text _textLabel2;
    [SerializeField] private TMP_Text _textLabel3;

    public Button SlotButton => _button;

    public void ShowInfoCharacterInfo(string str1, string str2, string str3)
    {
        _textLabel1.text = str1;
        _textLabel2.text = str2;
        _textLabel3.text = str3;

        _infoCharacterSlot.SetActive(true);
        _emtySlot.SetActive(false);

    }

    public void ShowEmptySlot()
    {
        _infoCharacterSlot.SetActive(false);
        _emtySlot.SetActive(true);
    }
}
