using System;
using System.Collections;
using System.Collections.Generic;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;

public class PointTextPool : LazySingleton<PointTextPool>
{
    public GameObject pointTextPrefab;
    public List<GameObject> pointTextPool;

    private void Start()
    {
        for (int i = 0; i < 150; i++)
        {
            GameObject text = Instantiate(pointTextPrefab, transform.position, Quaternion.Euler(0, -270, 0), transform);
            pointTextPool.Add(text);
        }
    }

    public GameObject GetReadyText()
    {
        if (pointTextPool.Count == 0)
        {
            for (int i = 0; i < 50; i++)
            {
                GameObject text = Instantiate(pointTextPrefab, transform.position, Quaternion.Euler(0, -270, 0), transform);
                pointTextPool.Add(text);
                GetReadyText();
            }
        }
        
        return pointTextPool[0];
    }
}
