using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform => playerTransform;

    private void Awake()
    {
        Init();   
    }

    public override void Init()
    {
        // 매니저들을 초기화 합니다.
        PoolManager.Instance.Init();
        InputHandler.Instance.Init();
    }
}
