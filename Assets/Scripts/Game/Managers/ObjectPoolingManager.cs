using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    Queue<GameObject> _poolingQueue = new Queue<GameObject>();
    public void Init(int maxCount, GameObject prefab)
    {
        for (int count = 0; count < maxCount; count++)
            _poolingQueue.Enqueue(CreateObject(prefab));
    }
    GameObject CreateObject(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(false);
        return newObj;
    }
    public GameObject Get()
    {
        if (_poolingQueue.Count > 0)
        {
            GameObject obj = _poolingQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }
        return null;
    }
    public void Return(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        _poolingQueue.Enqueue(obj);
    }
}
