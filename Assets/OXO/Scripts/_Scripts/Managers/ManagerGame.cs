using System;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using TMPro;
using UnityEngine;

public class ManagerGame : LazySingleton<ManagerGame>
{
    private int _number;

    [Tooltip("Oyun Level Ending e girdikten sonra bekmenmesi gereken sure")]
    public float LevelEndWaitDuration;

    [Tooltip("Ilk Bastaki Top Sayisi")]
    public float BeginBallCount;
    [Tooltip("Oyunun Icindeki Benim Gucum")]
    public float MyPower;
    public Vector3 ForcePos;
    public float GlassBrokeForce;
    [Tooltip("Topun Boing Efektindeki Buyume Faktoru")]
    public float BallScaleFactor;
    [Tooltip("Toplarin Diger Cage e girdikten sonra ziplama suresi")]
    public float BallJumpDuration;
    [Tooltip("Sayilarin textlerindeki azalma ve artmadaki sure")]
    public float NumberCountDuration;

    [Header("About Ball Movement Eases: ")]
    public Ease HorizontalEase;
    public Ease VerticalEase;
    
    [Header("About Ball Movement Speed: ")]
    public float HorizontalMoveSpeed;
    public float VerticalMovementSpeed;

    [Header("About All Ball Things: "), Space]
    public float BallForcePower;
    public float BallBoingDuration;
    public GameObject BallPrefab;
    public Color MyBallsFirstColor;
    public float ColorChangeDelay;
    public float BallWarDelay;
    
    public TextMeshProUGUI PlayerScoreText;

    public GameObject AllBallsParent;
    
    private void Start()
    {
        PlayerScoreText.text = MyPower.ToString();
    }

    public void UpdateScore(string str ,float value)
    {
        switch (str)
        {
            case "+":
                MyPower += value;
                break;
            case "-":
                MyPower -= value;
                break;
            case "*":
                MyPower *= value;
                break;
            case "/":
                MyPower /= value;
                break;
        }
        UpdateMyScoreText(true);
    }
    
    public void IncreaseMyPower(float value, CagePiece cagePiece)
    {
        DOTween.To(() => MyPower, (m) => MyPower = m, MyPower + value, cagePiece.cagePower * BallJumpDuration)
            .OnUpdate(() =>
            {
                UpdateMyScoreText(true);
            })
            .OnComplete(() =>
            {
                //TapManager.Instance.canTap = true;
            })
            .SetEase(Ease.Linear);
    }

    public void DecreaseMyPower(float value, CagePiece cagePiece)
    {
        DOTween.To(() => MyPower, (m) => MyPower = m, MyPower - value, cagePiece.cagePower * BallJumpDuration)
            .OnUpdate(() =>
            {
                UpdateMyScoreText(false);
            })
            .SetEase(Ease.Linear);
    }
    
    private void UpdateMyScoreText(bool success)
    {
        _number = (int)MyPower;
        PlayerScoreText.text = _number.ToString();
        
        if(success) return;
        if (_number <= 0)
        {
            _number = 0;
        }
    }
}
