using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    Queue<GameObject> _pool = new Queue<GameObject>();
    List<GameObject> _activatedGobjs = new List<GameObject>();
    public void Init(int maxCount, GameObject prefab)
    {
        for (int count = 0; count < maxCount; count++)
            _pool.Enqueue(CreateObject(prefab));
    }
    GameObject CreateObject(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(false);
        return newObj;
    }
    public GameObject Get()
    {
        if (_pool.Count > 0)
        {
            GameObject gobj = _pool.Dequeue();
            _activatedGobjs.Add(gobj);
            InitGobj(gobj);
            return gobj;
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
    void InitGobj(GameObject gobj)
    {
        gobj.transform.SetParent(null);
        gobj.SetActive(true);
    }
    public void Return(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);

        _pool.Enqueue(obj);
        _activatedGobjs.Remove(obj);
    }
}
