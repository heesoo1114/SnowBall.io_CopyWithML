using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    private Camera mainCam;

    public override void Init()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (mainCam != null)
        {
            
        }
    }
}
