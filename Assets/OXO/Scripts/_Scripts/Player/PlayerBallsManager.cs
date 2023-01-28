using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using MuhammetInce.HelperClass;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerBallsManager : LazySingleton<PlayerBallsManager>
{
    private ManagerGame GameManager;
    private Transform _firstSpawnLocation;
    
    public GameObject ballPrefab;
    public List<GameObject> ballList;

    private void Start()
    {
        GameManager = ManagerGame.Instance;
        _firstSpawnLocation = TapManager.Instance.startPos.transform;
        FirstBallSpawner();
    }
    
    private async void FirstBallSpawner()
    {
        await Task.Delay(100);
        GameManager.BeginBallCount = GameManager.MyPower;
        
        for (int i = 0; i < GameManager.BeginBallCount; i++)
        {
            GameObject ball = Instantiate(ballPrefab, _firstSpawnLocation.position, Quaternion.identity, GameManager.AllBallsParent.transform);
            ball.GetComponent<BallManager>().ColorChanger(GameManager.MyBallsFirstColor, 0);
            ballList.Add(ball);
        }
    }

    public void LevelEndBallJumper(GameObject bound)
    {
        foreach (GameObject go in ballList)
        {
            go.transform.DOJump(RandomPointInBounds(bound.GetComponent<Collider>().bounds, false, false), 1, 1,
                    GameManager.HorizontalMoveSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    go.GetComponent<Rigidbody>().AddForce(new Vector3(Random.value, Random.value, 0) * 200);
                    go.transform.DOScale(transform.localScale * 2, 0.5f)
                        .SetRelative()
                        .SetEase(Ease.Linear);
                });
        }
    }

    public void BallJumper(GameObject bound, bool isRight, bool isVertical = false, bool isGameObject = false)
    {
        if(!isGameObject)
        {
            foreach (GameObject go in ballList)
            {
                go.transform.DOJump(RandomPointInBounds(bound.GetComponent<Collider>().bounds, isRight, isVertical), 1, 1, GameManager.HorizontalMoveSpeed)
                    .SetEase(GameManager.HorizontalEase)
                    .OnComplete(() =>
                    {
                        if (isRight && !isVertical)
                        {
                            go.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.down) * GameManager.BallForcePower);
                        }
                        else if (!isRight && !isVertical)
                        {
                            go.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.down) * GameManager.BallForcePower);
                        }
                        else if (!isRight && isVertical)
                        {
                            go.GetComponent<Rigidbody>().AddForce((Vector3.right + Vector3.down) * GameManager.BallForcePower);
                        }
                    });
            }
        }
        else
        {           
            foreach (GameObject go in ballList)
            {
                go.transform.DOJump(bound.transform.position, 1, 1, GameManager.HorizontalMoveSpeed)
                    .SetEase(GameManager.HorizontalEase)
                    .OnComplete(() =>
                    {
                        if (isRight && !isVertical)
                        {
                            go.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.down) * GameManager.BallForcePower);
                        }
                        else if (!isRight && !isVertical)
                        {
                            go.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.down) * GameManager.BallForcePower);
                        }
                        else if (!isRight && isVertical)
                        {
                            go.GetComponent<Rigidbody>().AddForce((Vector3.right + Vector3.down) * GameManager.BallForcePower);
                        }
                    });
            }
        }
    }

    /*public void BallJumperV2(List<GameObject> list, bool isRight, bool isVertical = false)
    {
        foreach (GameObject go in ballList)
        {
            go.transform.DOJump(list[HelperClass.GetRandomInteger(0, list.Count)].transform.position, 1, 1, GameManager.HorizontalMoveSpeed)
                .SetEase(GameManager.HorizontalEase)
                .OnComplete(() =>
                {
                    if (isRight && !isVertical)
                    {
                        go.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.down) * GameManager.BallForcePower);
                    }
                    else if (!isRight && !isVertical)
                    {
                        go.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.down) * GameManager.BallForcePower);
                    }
                    else if (!isRight && isVertical)
                    {
                        go.GetComponent<Rigidbody>().AddForce((Vector3.right + Vector3.down) * GameManager.BallForcePower);
                    }
                });
        }
    }*/

    // public void DownForce(Vector3 hitPos)
    // {
    //     foreach (GameObject go in ballList)
    //     {
    //         go.GetComponent<Rigidbody>().AddForce(hitPos + new Vector3(0, -100,0));
    //     }
    // }

    public void BallMover(Vector3 target)
    {
        foreach (GameObject go in ballList)
        {
            // throwing up, keeping the position fixed as it is.
            //go.transform.DOMove(new Vector3(go.transform.position.x, target.y, go.transform.position.z), GameManager.VerticalMovementSpeed)
            
            // move forward by concentrating on the center.
            Vector3 targetCalculated = new Vector3(Random.Range((target.x - 0.5f), (target.x + 0.5f)),
                Random.Range(target.y, (target.y + 0.7f)), Random.Range((target.z - 0.3f), (target.z + 0.3f)));
            
            go.transform.DOMove(targetCalculated, GameManager.VerticalMovementSpeed)
                .SetEase(GameManager.VerticalEase)
                .SetSpeedBased()
                .OnComplete(() =>
                {
                    go.GetComponent<Rigidbody>().AddForce(HelperClass.UpAndLeftOrRight() * 50);
                });

            // Throwing up using pure physics.
            /*IEnumerator colliderToggle = go.GetComponent<Collider>().ColliderToggle(0.5f);
            StartCoroutine(colliderToggle);

            go.GetComponent<Rigidbody>().AddForce(Vector3.up * 420);*/
        }
    }
    
    private Vector3 RandomPointInBounds(Bounds bounds, bool right, bool isVertical)
    {
        float cutValue = 0.95f;
        
        if(!isVertical)
        {
            if(right)
            {
                return new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y - cutValue),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
            }
            else
            {
                return new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y - cutValue),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
            }
        }
        else
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y - cutValue),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}
