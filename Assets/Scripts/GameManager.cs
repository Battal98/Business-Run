using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Cinemachine;
//using ElephantSDK;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Bools
    [Header("-- Bools --")]
    public static bool isGameStarted = false;
    public static bool isGameEnded = false;
    public static bool isGameRestarted = false;
    #endregion

    #region GameObject, TextMeshPro and Lists
    [Header("-- Lists --")]
    public List<GameObject> Levels;
    [Space]
    [Header("-- Cams --")]
    public CinemachineVirtualCamera vCamGame;
    [Space]
    [Header(" *-_Player Values_-*")]
    public float PlayerForwardSpeed = 2;
    public int TotalMoney;
    public int currCoin;
    #endregion

    [Space]
    public int levelCount = 0;
    public int nextLevel = 0;
    [Header("-- GaneObjects --")]
    public GameObject confettiP;
    Volume volume;
    private Animator _iconAnim;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        volume = this.GetComponent<Volume>();
    }

    private void Start()
    {
        isGameStarted = false;
        isGameEnded = true;
        isGameRestarted = false;
        StartGame();
        UIManager.instance.LevelsText.text = "LEVEL" + (nextLevel + 1).ToString();
        _iconAnim = UIManager.instance.timerIcon.GetComponent<Animator>();
        confettiP = GameObject.FindGameObjectWithTag("Confetti");
    }
    public void StartGame()
    {
        if (isGameRestarted)
        {
            UIManager.instance.MainMenu.SetActive(false);
            volume.enabled = false;
        }
        levelCount = PlayerPrefs.GetInt("levelCount", levelCount);
        nextLevel = PlayerPrefs.GetInt("nextLevel", nextLevel);

        if (levelCount < 0 || levelCount >= Levels.Count)
        {
            levelCount = 0;
            PlayerPrefs.SetInt("levelCount", levelCount);
        }
        CreateLevel(levelCount);
        //Elephant.LevelStarted(nextLevel);
    }
    // Create Level.
    public void CreateLevel(int Levelindex)
    {
        Instantiate(Levels[Levelindex], new Vector3(0, 0, 0), Levels[Levelindex].transform.rotation);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGameEnded)
        {
            PlayerController.instance.CalculatedMouseDown();
            UIManager.instance.MainMenu.SetActive(false);
            UIManager.instance.GameMenu.SetActive(true);
            volume.enabled = false;
            isGameStarted = true;
            isGameEnded = false;
        }
    }
    public void OnLevelCompleted()
    {
        StartCoroutine(WaitForFinish(1f));
        confettiP.GetComponent<ParticleSystem>().Play();
        //Elephant.LevelCompleted(nextLevel);
    }
    //if Level Failed
    public void OnLevelFailed()
    {
        isGameEnded = true;
        isGameStarted = false;
        volume.enabled = true;
        _iconAnim.SetBool("Hurry", false);
        UIManager.instance.LoseMenu.SetActive(true);
        UIManager.instance.GameMenu.SetActive(false);
        //Elephant.LevelFailed(nextLevel);
    }
    // When Game is Start
    public void OnLevelEnded()
    {
        isGameEnded = true;
        Debug.Log("Level Bitti.");
        UIManager.instance.GameMenu.SetActive(false);
        _iconAnim.SetBool("Hurry", false);
        vCamGame.Follow = null;
    }
    public IEnumerator WaitForFinish(float _waitTime)
    {
        _iconAnim.SetBool("Hurry", false);
        isGameEnded = true;
        yield return new WaitForSeconds(_waitTime);
        UIManager.instance.WinMenu.SetActive(true);
        UIManager.instance.GameMenu.SetActive(false);
        volume.enabled = true;
    }
}