using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableController : MonoBehaviour
{
    public enum CollectableMode
    {
        None, Good, Bad
    }
    public CollectableMode mode = CollectableMode.Good;
    public enum CollectableType
    {
        Time, Coin
    }
    public CollectableType type = CollectableType.Time;

    [Space]
    [Header("** Time Properties **")]
    [SerializeField] GameObject ObjBody;
    [SerializeField] Material GoodMat;
    [SerializeField] Material BadMat;
    [SerializeField] public int TimeValue;
    [SerializeField] TextMeshPro TimeValueText;
    [SerializeField] Color GoodTextColor = Color.green;
    [SerializeField] Color BadTextColor = Color.red;
    [Space]
    [Header("** Coin Properties **")]
    public int CoinCost = 100;

    private void Start()
    {
        if (mode == CollectableMode.Good)
        {
            ObjBody.GetComponent<MeshRenderer>().material = GoodMat;
            TimeValueText.text = "+" + TimeValue.ToString();
            TimeValueText.color = GoodTextColor;
        }
        if (mode == CollectableMode.Bad)
        {
            ObjBody.GetComponent<MeshRenderer>().material = BadMat;
            TimeValueText.text = "-" + TimeValue.ToString();
            TimeValueText.color = BadTextColor;
        }
    }
}
