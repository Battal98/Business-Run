using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    private int _totalMoney;

    private void Start()
    {
        _totalMoney = PlayerPrefs.GetInt("TotalMoney", GameManager.instance.TotalMoney);
        UIManager.instance.totalMoneyText.text = _totalMoney.ToString();
    }
    public void MoneyIncreaseProcess(GameObject _obj)
    {

    }

    public void MoneyDecreaseProcess(GameObject _obj)
    {

    }

    public void CalculateTotalMoney()
    {

    }
}
