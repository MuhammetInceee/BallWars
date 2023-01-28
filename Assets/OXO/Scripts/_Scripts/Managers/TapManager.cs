using MuhammetInce.DesignPattern.Singleton;
using MoreMountains.NiceVibrations;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class TapManager : LazySingleton<TapManager>
{
    private RaycastHit _hitClick;
    private RaycastHit _hitCalculate;
    private float _enemyPower;
    private int _tempSuccess;
    private int _temp;
    private int _temp2;
    private ManagerGame GameManager;
    private GameObject _me;
    private GameObject _levelEndEntry;
    private GameObject _tempObj;
    private GameObject _targetBall;
    private GameObject _targetObj;
    
    public bool canTap = true;


    [SerializeField] private PlayerBallsManager playerBallsManager;
    [SerializeField] private CagePiece currentCage;
    [SerializeField] private CagePiece lastCage;
    [SerializeField] private Camera mainCamera;

    [Header("About Cameras"), Space] [SerializeField]
    private CinemachineVirtualCamera levelEndCamera;

    [SerializeField] private CinemachineVirtualCamera gameplayCamera;

    [Header("About Cage: "), Space] [SerializeField]
    private GameObject[] cageList;

    public GameObject startPos;

    [Header("About Layer Masks: "), Space] [SerializeField]
    private LayerMask cageMask;

    [SerializeField] private LayerMask cageableMask;

    private Ray Ray => mainCamera.ScreenPointToRay(Input.mousePosition);

    private void Start()
    {
        CageListFiller();
        GameManager = ManagerGame.Instance;
        _levelEndEntry = GameObject.FindGameObjectWithTag("LevelEnd");
        _me = GameObject.FindGameObjectWithTag("Player");
        _me.transform.position = startPos.transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!canTap) return;
            RaycastHitter();
        }
    }


    private void RaycastHitter()
    {
        if (!Physics.Raycast(Ray, out _hitClick, Mathf.Infinity, cageMask)) return;
        if (_hitClick.collider.gameObject.name == "Fog") return;

        if (currentCage != null)
        {
            lastCage = currentCage;
        }
        
        currentCage = _hitClick.collider.GetComponent<CagePiece>();

        Vector3 hitPos = _hitClick.transform.position;
        _targetObj = _hitClick.transform.gameObject;

        //cagePiece.BrokenGlassActivator();
        GameManager.ForcePos = hitPos - _me.transform.position;

        // TODO Player Movement Is Here
        canTap = false;

        if (currentCage != null)
        {
            _tempObj = currentCage.GetComponent<CagePiece>().bottomCollider;
            currentCage.operationGate?.SetColliderActive(true);
            // currentCage.operationGate.SetColliderActive(true);
        }


        // if me and my target in same column
        if (_me.transform.position.x == hitPos.x && _me.transform.position.z == hitPos.z)
        {
            // Go Top Movement
            if (hitPos.y > _me.transform.position.y)
            {
                playerBallsManager.BallMover(hitPos);
            }

            //Go Down Movement
            else
            {
                // // Fully physic movement
                // IEnumerator bottom = _tempObj.GetComponent<Collider>().ColliderToggle(1f);
                // StartCoroutine(bottom);
                //
                // IEnumerator top = currentCage.GetComponent<CagePiece>().topCollider.GetComponent<Collider>()
                //     .ColliderToggle(1f);
                // StartCoroutine(top);
                //
                // playerBallsManager.DownForce(hitPos);

                // New Down Movement
                playerBallsManager.BallMover(hitPos);
            }

            // Main Player Movement
            _me.transform.DOMove(hitPos, GameManager.VerticalMovementSpeed)
                .OnComplete(EnemyCageSituation)
                .SetSpeedBased()
                .SetEase(GameManager.VerticalEase);
        }

        // player and his target have not same column
        else
        {
            // horizontal Movement

            // Left Movement
            if (_me.transform.position.x == hitPos.x && _me.transform.position.z < hitPos.z)
            {
                // Left Move
                switch (playerBallsManager.ballList.Count)
                {
                    case <= 10 when currentCage.ballList.Count > 0:
                        print("1");
                        playerBallsManager.BallJumper(_targetObj, false, false, true);
                        break;
                    case > 10 when currentCage.ballList.Count > 0:
                        print("2");
                        playerBallsManager.BallJumper(_targetObj, false);
                        break;
                    case > 10 when currentCage.ballList.Count == 0:
                        print("3");
                        playerBallsManager.BallJumper(_targetObj, false);
                        break;
                    case <= 10 when currentCage.ballList.Count == 0:
                        print("4");
                        playerBallsManager.BallJumper(_targetObj, false, false, true);
                        break;
                }
            }
            // Right Movement
            else if (_me.transform.position.x == hitPos.x && _me.transform.position.z > hitPos.z)
            {
                // Right Move
                switch (playerBallsManager.ballList.Count)
                {
                    case <= 10 when currentCage.ballList.Count > 0:
                        playerBallsManager.BallJumper(_targetObj, true, false, true);
                        break;
                    case > 10 when currentCage.ballList.Count > 0:
                        playerBallsManager.BallJumper(_targetObj, true);
                        break;
                    case > 10 when currentCage.ballList.Count == 0:
                        playerBallsManager.BallJumper(_targetObj, true);
                        break;
                    case <= 10 when currentCage.ballList.Count == 0:
                        playerBallsManager.BallJumper(_targetObj, true, false, true);
                        break;
                }
                //playerBallsManager.BallJumper(_targetObj, true);
            }
            // Other Movement
            else
            {
                switch (playerBallsManager.ballList.Count)
                {
                    case <= 10 when currentCage.ballList.Count > 0:
                        playerBallsManager.BallJumper(_targetObj, false, true, true);
                        break;
                    case > 10 when currentCage.ballList.Count > 0:
                        playerBallsManager.BallJumper(_targetObj, false, true);
                        break;
                    case > 10 when currentCage.ballList.Count == 0:
                        playerBallsManager.BallJumper(_targetObj, false, true);
                        break;
                    case <= 10 when currentCage.ballList.Count == 0:
                        playerBallsManager.BallJumper(_targetObj, false, true, true);
                        break;
                }
            }

            _me.transform.DOJump(hitPos, 0.55f, 1, GameManager.HorizontalMoveSpeed)
                .OnComplete(EnemyCageSituation)
                .SetEase(GameManager.HorizontalEase);
        }
    }


    private IEnumerator BallColorChanger(GameObject targetObj, bool isSuccess)
    {
        yield return new WaitForSeconds(0.1f);
        CagePiece cagePiece = targetObj.GetComponent<CagePiece>();

        if (isSuccess)
        {
            foreach (GameObject obj in cagePiece.ballList)
            {
                BallManager ballManager = obj.GetComponent<BallManager>();

                obj.transform.DOJump(playerBallsManager.ballList
                            [Random.Range(0, playerBallsManager.ballList.Count)].transform.position,
                        0.2f
                        , 1, GameManager.BallJumpDuration)
                    .OnComplete(() =>
                    {
                        playerBallsManager.ballList.Add(obj);
                        ballManager.ColorChanger(GameManager.MyBallsFirstColor, GameManager.ColorChangeDelay);
                        ballManager.BoingEffect();
                        obj.GetComponent<TrailRenderer>().enabled = true;
                    });
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                yield return new WaitForSeconds(GameManager.BallWarDelay);
            }

            canTap = true;
            cagePiece.ballList.Clear();
            //OperationActivator(cagePiece);
            if (targetObj.name.Contains("Final"))
            {
                StartCoroutine(FinalCageSituation());
            }
        }
        else
        {
            canTap = false;
            // foreach (GameObject go in cagePiece.ballList)
            // {
            //     GameObject obj = playerBallsManager.ballList[Random.Range(0, playerBallsManager.ballList.Count)];
            //     
            //     go.transform.DOJump(obj.transform.position,0.2f,1, 0.2f)
            //         .OnComplete(() =>
            //         {
            //             BallManager ballManager = obj.GetComponent<BallManager>();
            //             
            //             ballManager.ColorChanger(cagePiece.startColor, GameManager.ColorChangeDelay);
            //             ballManager.BoingEffect();
            //         });
            //     yield return new WaitForSeconds(GameManager.BallWarDelay);
            // }

            for (int i = 0; i < cagePiece.ballList.Count; i++)
            {
                if (playerBallsManager.ballList.Count == 0)
                {
                    break;
                }
                else
                {
                    _targetBall = playerBallsManager.ballList[Random.Range(0, playerBallsManager.ballList.Count)];
                }

                cagePiece.ballList[i].transform
                    .DOJump(_targetBall.transform.position, 0.2f, 1, GameManager.BallJumpDuration)
                    .OnComplete(() =>
                    {
                        BallManager ballManager = _targetBall.GetComponent<BallManager>();

                        ballManager.ColorChanger(cagePiece.startColor, GameManager.ColorChangeDelay);
                        ballManager.BoingEffect();

                        _targetBall.GetComponent<TrailRenderer>().enabled = false;
                        playerBallsManager.ballList.Remove(_targetBall);
                    });
                yield return new WaitForSeconds(GameManager.BallWarDelay);
            }

            canTap = false;
        }
    }

    private float _tempDuration;

    private IEnumerator FinalCageSituation()
    {
        _tempDuration = GameManager.LevelEndWaitDuration;

        yield return new WaitForSeconds(0.6f);

        _me.transform.DOMove(_levelEndEntry.transform.position, 1f)
            .OnComplete(() =>
            {
                _me.gameObject.SetActive(false);
                gameplayCamera.gameObject.SetActive(false);
                levelEndCamera.gameObject.SetActive(true);
            });
        playerBallsManager.LevelEndBallJumper(_levelEndEntry);
        Physics.gravity *= 2;
        canTap = false;

        while (_tempDuration > 0)
        {
            _tempDuration -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        CanvasManager.Instance.OpenFinishRect(true);
    }

    private void EnemyCageSituation()
    {
        //AllCagesCageLayerChanger();
        //CageCalculatorToGo(); 

        CagePiece cagePiece = _hitClick.collider.GetComponent<CagePiece>();

        TextMeshProUGUI cageText = cagePiece.cagePowerText;
        TextMeshProUGUI playerScoreText = GameManager.PlayerScoreText;


        // Success Movement Case
        if (GameManager.MyPower >= cagePiece.cagePower)
        {
            StartCoroutine(BallColorChanger(currentCage.gameObject, true));
            GameManager.IncreaseMyPower(cagePiece.cagePower, cagePiece);
            cagePiece.cagePower = 0;
            cageText.gameObject.SetActive(false);
            //NumberCounterSuccess(cagePiece.cagePower, cageText, cagePiece);
        }

        // Level Fail Case
        else if (GameManager.MyPower < cagePiece.cagePower)
        {
            StartCoroutine(BallColorChanger(currentCage.gameObject, false));
            GameManager.DecreaseMyPower(cagePiece.cagePower, cagePiece);
            NumberCounterFail(GameManager.MyPower, playerScoreText, cagePiece);
            //textObj.SetActive(false);
        }
        
        if(lastCage == null) return;
        OperationActivator(lastCage);
    }

    private void OperationActivator(CagePiece cagePiece)
    {
        if (!cagePiece.isOperatorCage) return;
        cagePiece.SetOperatorModel();
    }

    // private void NumberCounterSuccess(float cagePower, TextMeshProUGUI text, CagePiece cagePiece)
    // {
    //     DOTween.To(() => cagePower, (m) => cagePower = m, 0, cagePiece.cagePower * GameManager.BallJumpDuration)
    //         .OnUpdate(() =>
    //         {
    //             _tempSuccess = (int)cagePower;
    //             text.text = _tempSuccess.ToString();
    //         })
    //         .OnComplete(() =>
    //         {
    //             text.gameObject.SetActive(false);
    //             cagePiece.cagePower = 0;
    //             canTap = true;
    //         })
    //         .SetEase(Ease.Linear);
    // }


    private void NumberCounterFail(float targetPower, TextMeshProUGUI text, CagePiece cagePiece)
    {
        DOTween.To(() => targetPower, (m) => targetPower = m, targetPower - cagePiece.cagePower,
                cagePiece.cagePower * GameManager.BallJumpDuration)
            .OnUpdate(() =>
            {
                _temp = (int)targetPower;
                text.text = _temp.ToString();
                if (_temp <= 0)
                {
                    text.gameObject.SetActive(false);
                }
            })
            .OnComplete(() =>
            {
                text.gameObject.SetActive(false);
                GameManager.MyPower = 0;
                CanvasManager.Instance.OpenFinishRect(false);
                canTap = false;
            })
            .SetEase(Ease.Linear);

        cagePiece.cagePowerText.gameObject.SetActive(false);
        // DOTween.To(() => cagePiece.cagePower, (m) => cagePiece.cagePower = m, cagePiece.cagePower + GameManager.MyPower
        //         , cagePiece.cagePower * GameManager.BallJumpDuration)
        //     .OnUpdate(() =>
        //     {
        //         _temp2 = (int)cagePiece.cagePower;
        //         cagePiece.cagePowerText.text = _temp2.ToString();
        //     })
        //     .SetEase(Ease.Linear);
    }


    // private void CageCalculatorToGo()
    // {
    //     if (currentCage == null) return;
    //
    //     if (Physics.Raycast(currentCage.transform.position, Vector3.right, out _hitCalculate, Mathf.Infinity,
    //             cageableMask))
    //     {
    //         _hitCalculate.collider.gameObject.layer = 6;
    //     }
    //
    //     if (Physics.Raycast(currentCage.transform.position, Vector3.forward, out _hitCalculate, Mathf.Infinity,
    //             cageableMask))
    //     {
    //         _hitCalculate.collider.gameObject.layer = 6;
    //     }
    //
    //     if (Physics.Raycast(currentCage.transform.position, Vector3.back, out _hitCalculate, Mathf.Infinity,
    //             cageableMask))
    //     {
    //         _hitCalculate.collider.gameObject.layer = 6;
    //     }
    //
    //     if (Physics.Raycast(currentCage.transform.position, Vector3.up, out _hitCalculate, Mathf.Infinity,
    //             cageableMask))
    //     {
    //         _hitCalculate.collider.gameObject.layer = 6;
    //     }
    //
    //     if (Physics.Raycast(currentCage.transform.position, Vector3.down, out _hitCalculate, Mathf.Infinity,
    //             cageableMask))
    //     {
    //         _hitCalculate.collider.gameObject.layer = 6;
    //     }
    // }

    private void CageListFiller()
    {
        cageList = GameObject.FindGameObjectsWithTag("Cage");
    }

    // private void AllCagesCageLayerChanger()
    // {
    //     foreach (GameObject obj in cageList)
    //     {
    //         if (obj.layer == 6)
    //         {
    //             obj.layer = 7;
    //         }
    //     }
    // }
}