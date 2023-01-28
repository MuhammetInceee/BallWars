using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
public class CanvasManager : Singleton<CanvasManager>
{
    [HideInInspector]
    public static CanvasManager instance;

    public GameObject tapToPlayButton;
    public GameObject nextLevelButton;
    public GameObject retryLevelButton;

    public GameObject tutorialRect;
    public GameObject mainMenuRect;
    public GameObject inGameRect;
    public GameObject finishRect;

    public Image levelSliderImage;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;

    public GameObject winPanel;
    public GameObject failPanel;

    private void Awake()
    {
        mainMenuRect.SetActive(true);
    }
    public void TapToPlayButtonClick()
    {
        GameManager.instance.StartGame();
    }

    [Button]
    public void NextLevel()
    {
        LevelManager.instance.IncreaseLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //LevelManager.instance.SetLevel();

    }
    public void RestartGame()
    {
        LevelManager.instance.SetLevel();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    
    public void OpenFinishRect(bool isSuccess)
    {

        if (isSuccess)
        {
            winPanel.SetActive(true);
            retryLevelButton.SetActive(false);
            //nextLevelButton.SetActive(true);
            ScoreManager.Instance.SetNewHighScore();
        }
        else
        {
            failPanel.SetActive(true);
            retryLevelButton.SetActive(true);
            nextLevelButton.SetActive(false);
        }

        inGameRect.SetActive(false);
        finishRect.GetComponent<CanvasGroup>().DOFade(1, 2f);
        TapManager.Instance.canTap = false;
        finishRect.SetActive(true);
    }

    public void LevelResetAQ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}