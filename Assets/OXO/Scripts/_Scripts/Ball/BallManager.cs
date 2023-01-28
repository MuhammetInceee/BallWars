using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using MuhammetInce.HelperClass;
using TMPro;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private Vector3 _firstScale;
    private float _boingDuration;
    private Rigidbody _rb;
    private Collider _col;
    
    private Material _mat;
    private TrailRenderer _trailRenderer;
    private ScoreManager ScoreManager;
    private ManagerGame GameManager;
    private static readonly int Color02 = Shader.PropertyToID("_Color02");
    private static readonly int Slider = Shader.PropertyToID("_Slider");

    private void Initialize()
    {
        _mat = GetComponent<Renderer>().material;
        _trailRenderer = GetComponent<TrailRenderer>();
        GameManager = ManagerGame.Instance;
        ScoreManager = ScoreManager.Instance;
        _boingDuration = GameManager.BallBoingDuration;
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("X"))
        {
            BallDestroyAnim();
            GameObject text = PointTextPool.Instance.GetReadyText();
            TextCalculator(other.gameObject.name, text.GetComponent<TextMeshPro>());
            text.GetComponent<PointText>().Move(gameObject);
            ScoreManager.AddScore(other.gameObject.name);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
    }

    private void OnEnable()
    {
        Initialize();
        
        _firstScale = GameManager.BallPrefab.transform.localScale;
        _trailRenderer.material.color = GameManager.MyBallsFirstColor;
    }
    
    public void ColorChanger(Color targetColor, float delay)
    {
        _mat.SetColor(Color02, targetColor);
        _mat.DOFloat(1, Slider, delay); 
    }

    public void BoingEffect()
    {
        transform.DOScale(_firstScale * GameManager.BallScaleFactor, _boingDuration / 2)
            .OnComplete(() =>
            {
                transform.DOScale(_firstScale, _boingDuration / 2);
            });
    }

    public void BallSpawnAnim()
    {
        gameObject.transform.localScale = Vector3.zero;

        transform.DOScale(_firstScale, _boingDuration).OnComplete(() =>
        {
            PlayerBallsManager.Instance.ballList.Add(gameObject);
            ColorChanger(GameManager.MyBallsFirstColor, 0);
            _rb.isKinematic = false;
            _rb.AddForce(HelperClass.RandomizedVector3() * 130);
        });
    }

    public void BallDestroyAnim()
    {
        transform.DOScale(Vector3.zero, _boingDuration)
            .OnComplete(() =>
            {
                PlayerBallsManager.Instance.ballList.Remove(gameObject);
                Destroy(gameObject);
                if (PlayerBallsManager.Instance.ballList.Count == 0)
                {
                    CanvasManager.Instance.OpenFinishRect(true);
                }
            });
    }

    private void TextCalculator(string targetName, TextMeshPro text)
    {
        switch (targetName)
        {
            case "Center":
                text.text = "+" + ScoreManager.centerPointScore;
                break;
            case "Green":
                text.text = "+" + ScoreManager.greenPointScore;
                break;
            case "Orange":
                text.text = "+" + ScoreManager.orangePointScore;
                break;
            case "Red":
                text.text = "+" + ScoreManager.redPointScore;
                break;
            default:
                text.text = text.text;
                break;
        }
    }
}
