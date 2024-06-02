using System.Net.NetworkInformation;
using System.Collections;
using UnityEngine;
using System;

public static class MonoExtension
{
    public static void GiveDelayWithAction(this MonoBehaviour mono, float delayTime, Action action)
    {
        mono.StartCoroutine(DelayCoroutine(delayTime, action));
    }

    private static IEnumerator DelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static bool IsNetworkAvailable(this MonoBehaviour mono)
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }
}
