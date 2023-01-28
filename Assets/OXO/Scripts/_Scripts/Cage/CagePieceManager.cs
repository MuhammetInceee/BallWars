using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MText;
using TMPro;
using UnityEngine;
using MuhammetInce.HelperClass;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class CagePieceManager : MonoBehaviour
{
    private Vector3 startPos;

    public bool isOperatorCage;

    //[ShowIf("isOperatorCage")] public CagePiece targetCage;
    [ShowIf("isOperatorCage")] public Operations operations;
    [ShowIf("isOperatorCage")] public int value;
    [ShowIf("isOperatorCage")] public bool canUse;
    public OperationGate operationGate;

    private ManagerGame GameManager;
    private List<Color> _cageColorList;
    public List<GameObject> _threeDTextList;
    private GameObject _threeDParent;

    public Color startColor;

    [Header("About Cage Colliders: "), Space]
    public GameObject bottomCollider;

    public GameObject topCollider;

    [Header("About Own Ball: "), Space] public List<GameObject> ballList;

    [Header("About Cage Settings: "), Space]
    public float cagePower;

    [Header("About Cage UI: "), Space] public TextMeshProUGUI cagePowerText;


    private static readonly int Color01 = Shader.PropertyToID("_Color01");


    private void Start()
    {
        if (gameObject.name == "StartCage") return;
        Initialize();
        SpawnOwnBall();
        TextListFiller();
        OperatorCageInitialize();
    }

    private void Initialize()
    {
        GameManager = ManagerGame.Instance;
        _cageColorList = ColorManager.Instance.cageColorList;
        startColor = _cageColorList[Random.Range(0, _cageColorList.Count)];
        cagePowerText.text = cagePower.ToString();
        _threeDParent = GameObject.FindGameObjectWithTag("3DParent");
        canUse = true;
    }

    private void OperatorCageInitialize()
    {
        if (!isOperatorCage) return;
        startPos = _threeDTextList[0].transform.position;
    }

    private async void SpawnOwnBall()
    {
        await Task.Delay(100);

        for (int i = 0; i < cagePower; i++)
        {
            GameObject ball = Instantiate(GameManager.BallPrefab, transform.position, Quaternion.identity,
                GameManager.AllBallsParent.transform);
            ball.GetComponent<Renderer>().material.SetColor(Color01, startColor);
            ball.GetComponent<Rigidbody>().AddForce(HelperClass.RandomizedVector3() * 5);
            ball.GetComponent<TrailRenderer>().enabled = false;
            ballList.Add(ball);
        }
    }

    private void TextListFiller()
    {
        for (int i = 0; i < _threeDParent.transform.childCount; i++)
        {
            _threeDTextList.Add(_threeDParent.transform.GetChild(i).gameObject);
        }
    }

    public void SetOperatorModel()
    {
        if (!canUse) return;

        GameObject targetText = _threeDTextList
            .Where(m => m.activeInHierarchy)
            .FirstOrDefault(i => i.transform.position == startPos);

        Modular3DText modular3DText = targetText.GetComponent<Modular3DText>();

        SetOperator(targetText.GetComponent<OperationGate>(), modular3DText);
        canUse = false;
    }

    private void SetOperator(OperationGate _operationGate, Modular3DText modular3DText)
    {
        operationGate = _operationGate;
        operationGate.value = value;
        operationGate.operations = operations;

        modular3DText.transform.position = transform.position;
        modular3DText.GetComponent<Rotator>().enabled = true;

        modular3DText.Text = operations switch
        {
            Operations.Addition => "+" + value,
            Operations.Subtraction => "-" + value,
            Operations.Multiplication => "x" + value,
            Operations.Divide => "÷" + value,
            _ => modular3DText.Text
        };
        operationGate.SetColliderActive(false);
    }
}