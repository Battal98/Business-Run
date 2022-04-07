using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("-- UI Menus --")]
    public GameObject Canvas;
    public GameObject MainMenu;
    public GameObject GameMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public TextMeshProUGUI LevelsText;
    [Space]
    [Header("-- Money Texts --")]
    public TextMeshProUGUI currCoinText;
    public TextMeshProUGUI totalMoneyText;
    [Space]
    [Header("-- Time Staff --")]
    public Image timerFill;
    public Image timerIcon;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void StartTheGameButton()
    {
        UIManager.instance.MainMenu.SetActive(false);
        UIManager.instance.WinMenu.SetActive(false);
        GameManager.isGameStarted = true;
        GameManager.isGameEnded = false;
    }
    // Next Level Button
    public void NextLevelButton()
    {
        GameManager.isGameEnded = false;
        GameManager.isGameRestarted = true;
        GameManager.isGameStarted = true;
        GameManager.instance.levelCount++;
        GameManager.instance.nextLevel++;
        PlayerPrefs.SetInt("levelCount", GameManager.instance.levelCount);
        PlayerPrefs.SetInt("nextLevel", GameManager.instance.nextLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Rest.
    public void RestartButton()
    {
        GameManager.isGameRestarted = true;
        GameManager.isGameStarted = true;
        GameManager.isGameEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
