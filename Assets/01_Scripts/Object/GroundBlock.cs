using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    [SerializeField] private float targetPosY = -5f;
    private Vector3 targetPos;

    private bool isFall = false;

    private void Awake()
    {
        targetPos = transform.localPosition;
        targetPos.y += targetPosY;
    }

    private void Update()
    {
        if (isFall)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime); 
        }
    }

    public void Fall()
    {
        isFall = true;
    }
}
