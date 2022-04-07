using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float LevelTime = 40.0f;
    public float timer;
    public List<GameObject> GateLists = new List<GameObject>();

    private Animator _iconAnim;
    private float _hurryUpTimeBorder;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _iconAnim = UIManager.instance.timerIcon.GetComponent<Animator>();
        _hurryUpTimeBorder = LevelTime / 3f;
        timer = LevelTime;
    }
    private void Start()
    {
        GameObject[] _gateList = GameObject.FindGameObjectsWithTag("Gate");
        for (int i = 0; i < _gateList.Length; i++)
        {
            GateLists.Add(_gateList[i].gameObject);
        }

    }

    private void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
            return;
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            UIManager.instance.timerFill.fillAmount -= 1f / LevelTime * Time.deltaTime;
            if (timer < _hurryUpTimeBorder)
            {
                _iconAnim.SetBool("Hurry", true);
            }
            if (timer > LevelTime)
            {
                timer = LevelTime;
            }
        }
        else
        {
            _iconAnim.SetBool("Hurry", false);
            GameManager.instance.OnLevelFailed();
        }
    }
}
