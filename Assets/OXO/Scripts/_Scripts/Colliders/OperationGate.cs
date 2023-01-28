using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum Operations
{
    Addition,
    Subtraction,
    Multiplication,
    Divide
}

public class OperationGate : MonoBehaviour
{
    private ManagerGame _gameManager;
    private PlayerBallsManager _playerBallsManager;
    private TextMeshProUGUI _text;
    private Collider Collider;
    [HideInInspector] public Operations operations;
    [HideInInspector] public int value;
    private static readonly int Color01 = Shader.PropertyToID("Color01");

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _gameManager = ManagerGame.Instance;
        _playerBallsManager = PlayerBallsManager.Instance;
    }

    public void SetColliderActive(bool active)
    {
        Collider.enabled = active;
    }

    public void LetsCalculate()
    {
        float startScore = _gameManager.MyPower;
        
        switch (operations)
        {
            case Operations.Addition:
                _gameManager.UpdateScore("+", value);
                StartCoroutine(BallSpawner(value, transform));
                break;
            case Operations.Subtraction:
                _gameManager.UpdateScore("-", value);
                StartCoroutine(BallDestroyer(value));
                break;
            case Operations.Multiplication:
                _gameManager.UpdateScore("*", value);
                StartCoroutine(BallSpawner((int)(_gameManager.MyPower - startScore), transform));
                break;
            case Operations.Divide:
                _gameManager.UpdateScore("/", value);
                StartCoroutine(BallDestroyer((int)(startScore - _gameManager.MyPower)));
                break;
        }

        MeshRenderer mesh = GetComponent<MeshRenderer>();

        if (mesh != null)
        {
            mesh.enabled = false;
        }
        else
        {
            foreach (Transform tr in transform)
            {
                tr.gameObject.SetActive(false);
            }
        }
        
        GetComponent<Collider>().enabled = false;
    }
    
    private IEnumerator BallSpawner(int count, Transform tr)
    {
        for (int i = 0; i < count; i++)
        {
            
            GameObject ball = Instantiate(_gameManager.BallPrefab, tr.position, Quaternion.identity,
                _gameManager.AllBallsParent.transform);
            BallManager ballManager = ball.GetComponent<BallManager>();
            //ball.GetComponent<Rigidbody>().isKinematic = true;
            ballManager.BallSpawnAnim();
            ballManager.ColorChanger(ManagerGame.Instance.MyBallsFirstColor, 0);
            yield return new WaitForFixedUpdate();
        }
    }
    
    public IEnumerator BallDestroyer(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_playerBallsManager.ballList.Count > 0)
            {
                _playerBallsManager.ballList[i].GetComponent<BallManager>().BallDestroyAnim();
                yield return new WaitForFixedUpdate();
            }
            else
            {
                //TODO Level End
                CanvasManager.Instance.OpenFinishRect(false);
            }
        }
    }
}
