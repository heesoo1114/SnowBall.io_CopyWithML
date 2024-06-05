using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGround : MonoBehaviour
{
    private GroundBlock[] groundBlocks;

    [SerializeField] private float delayTime = 20f;

    private int baseGroundIndex;
    private int frontIndex;
    private int backIndex;

    private void Awake()
    {
        groundBlocks = GetComponentsInChildren<GroundBlock>();

        baseGroundIndex = Random.Range(1, groundBlocks.Length);
        frontIndex = 0;
        backIndex = groundBlocks.Length - 1;
    }

    private void Start()
    {
        StartCoroutine(GroundFallingLoop());
    }

    private IEnumerator GroundFallingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);

            if (frontIndex != baseGroundIndex)
            {
                groundBlocks[frontIndex].Fall();
                frontIndex++;
            }

            if (backIndex != baseGroundIndex)
            {
                groundBlocks[backIndex].Fall();
                backIndex--;
            }

            if ((frontIndex + backIndex) == (baseGroundIndex * 2))
            {
                yield break;
            }
        }
    }
}
