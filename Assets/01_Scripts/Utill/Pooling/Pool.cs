using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PoolableMono
{
    private Stack<T> pool = new Stack<T>();
    private T prefab;
    private Transform parentTr;

    public Pool(T prefab, Transform parent, int count = 10)
    {
        this.prefab = prefab;
        this.parentTr = parent;

        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(this.prefab, this.parentTr);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
            obj.gameObject.SetActive(false);
            pool.Push(obj);
        }
    }

    public T Pop()
    {
        T obj = null;

        // 남아있지 않으면 추가
        if (pool.Count <= 0)
        {
            obj = GameObject.Instantiate(prefab, parentTr);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
        }
        // 남아있으니 빼오기
        else
        {
            obj = pool.Pop();
            obj.gameObject.SetActive(true);
        }

        obj.OnPop();
        return obj;
    }

    public void Push(T obj)
    {
        obj.OnPush();
        obj.transform.SetParent(parentTr);
        obj.gameObject.SetActive(false);
        pool.Push(obj);
    }
}