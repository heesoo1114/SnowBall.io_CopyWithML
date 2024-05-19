using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private void Awake()
    {
        Init();   
    }

    public override void Init()
    {
        // �Ŵ������� �ʱ�ȭ �մϴ�.
        PoolManager.Instance.Init();
    }
}
