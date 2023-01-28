using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool isText;

    [EnableIf("@isText == false")] public float rouletteRotateSpeed;
    
    [ShowIf("isText")] public float textRotateSpeed;
    [ShowIf("isText")] public float minAngle;
    [ShowIf("isText")] public float maxAngle;

    private Vector3 _tempVector;
    

    private void Start()
    {
        _tempVector = Vector3.up;
    }

    void Update()
    {
        if (isText)
        {
            transform.Rotate(_tempVector * (textRotateSpeed * Time.deltaTime));
            VectorChanger();
        }
        else
        {
            transform.Rotate(Vector3.up * (rouletteRotateSpeed * Time.deltaTime));
        }
    }

    private void VectorChanger()
    {
        if (transform.localEulerAngles.y >= maxAngle)
        {
            _tempVector = Vector3.down;
        }
        
        if (transform.localEulerAngles.y <= minAngle)
        {
            _tempVector = Vector3.up;
        }
    }
}
