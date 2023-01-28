using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using TMPro;
using UnityEngine;

public class ScoreManager : LazySingleton<ScoreManager>
{
    private int _tempScore;
    
    public float counterDuration;

    [Header("About General Canvases: ")] 
    public GameObject NextLevelButton;
    
    [Header("About Score UI")] 
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI newHighScoreText;
    
    [Header("About Score And High Score: ")]
    public int HighScore;
    public int currentScore;
    
    
    [Header("Hole Scores: ")]
    public int greenPointScore;
    public int orangePointScore;
    public int redPointScore;
    public int centerPointScore;

    private void Start()
    {
        HighScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "Pb: " + HighScore;
    }

    
    private void UpdateHighScore(int value)
    {
        HighScore = value;
        PlayerPrefs.SetInt("HighScore", HighScore);
    }

    
    public void SetNewHighScore()
    {
        DOTween.To(() => _tempScore, (m) => _tempScore = m, currentScore, counterDuration)
            .OnUpdate(() =>
            {
                currentScoreText.text = _tempScore.ToString();
            })
            .OnComplete(() =>
            {
                if (currentScore > HighScore)
                {
                    UpdateHighScore(currentScore);
                    ConfettiManager.instance.Play();
                    newHighScoreText.gameObject.SetActive(true);
                }
                NextLevelButton.SetActive(true);
            })
            .SetEase(Ease.Linear);
    }
    
    private void AddCurrentScore(int value)
    {
        currentScore += value;
    }

    public void AddScore(string color)
    {
        switch (color)
        {
            case "Center":
                AddCurrentScore(centerPointScore);
                break;
            
            case "Pink":
                AddCurrentScore(redPointScore);
                break;
            
            case "Green":
                AddCurrentScore(greenPointScore);
                break;
            
            case "Orange":
                AddCurrentScore(orangePointScore);
                break;
        }
    }
    
}
