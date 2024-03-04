using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    Queue<GameObject> _poolingQueue = new Queue<GameObject>();
    List<GameObject> _activatedGobjs = new List<GameObject>();
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
            GameObject gobj = _poolingQueue.Dequeue();
            _activatedGobjs.Add(gobj);

            return InitGobj(gobj);
        }
        else
        {
            foreach (GameObject gobj in _activatedGobjs)
            {
                if (gobj.transform.parent == null)
                    Return(gobj);
                    return Get();
            }
        }
        return null;
    }
    GameObject InitGobj(GameObject gobj)
    {
        gobj.transform.SetParent(null);
        gobj.SetActive(true);
        return gobj;
    }
    public void Return(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);

        _poolingQueue.Enqueue(obj);
        _activatedGobjs.Remove(obj);
    }
}
