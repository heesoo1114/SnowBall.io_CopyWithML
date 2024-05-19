using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public string Id;
    public abstract void OnPop();
    public abstract void OnPush();
}