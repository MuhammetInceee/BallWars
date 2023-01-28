using System;
using UnityEngine;

public class BallsParentCollider : MonoBehaviour
{
    private ManagerGame GameManager;
    
    private OperationGate _operation;

    private void Start()
    {
        GameManager = ManagerGame.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Multipliers"))
        {
            other.GetComponent<OperationGate>().LetsCalculate();
        }
        
        else if (other.TryGetComponent(out WallBangBang bang))
        {
            bang.BangBang(GameManager.GlassBrokeForce, GameManager.ForcePos);
            other.GetComponent<Collider>().enabled = false;
        }
        
    }
}
