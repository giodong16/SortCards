using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinDialog : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    public void SetUp(int money)
    {
        moneyText.text = money.ToString();
    }
}
