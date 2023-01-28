using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallBangBang : MonoBehaviour
{
    public List<Rigidbody> piece;
    public GameObject solidObj;
    
    private void Awake()
    {
        foreach (Transform tr in transform)
        {
            piece.Add(tr.gameObject.GetComponent<Rigidbody>());
            
        }

        foreach (Rigidbody rb in piece)
        {
            rb.isKinematic = true;
            rb.gameObject.SetActive(false);
        }
    }

    public void BangBang(float Force, Vector3 pos)
    {
        foreach (Rigidbody rb in piece)
        {
            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.AddForce(pos * Force, ForceMode.Force);
            Destroy(rb.gameObject, Random.Range(1f, 3f));
        }
    }

    private void InvisibleWallForBalls()
    {
        solidObj.GetComponent<MeshRenderer>().enabled = false;
    }
}
