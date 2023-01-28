using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PointText : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(_mainCamera.transform);
    }

    public void Move(GameObject target)
    {
        transform.position = target.transform.position;
        PointTextPool.Instance.pointTextPool.Remove(gameObject);
        transform.DOMoveY(25, 1f)
            .SetRelative()
            .OnComplete(() =>
            {
                PointTextPool.Instance.pointTextPool.Add(gameObject);
                transform.localPosition = Vector3.zero;
            });
    }
}
