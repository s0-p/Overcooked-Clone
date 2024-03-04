using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTable : BasicTable
{
    public override void PutOnObject(Transform objectTransform)
    {
        base.PutOnObject(objectTransform);
        StartCoroutine(CRT_ThrowAway());
    }
    IEnumerator CRT_ThrowAway()
    {
        Vector3 originScale = OnObject.transform.localScale;
        while (OnObject.transform.localScale.x > 0.1)
        {
            OnObject.transform.localScale *= 0.8f;
            OnObject.transform.Rotate(Vector3.up * 30f);
            yield return new WaitForSeconds(0.1f);
        }
        OnObject.transform.localScale = originScale;
        OnObject.transform.parent = null;

        IngredientCtrl ctrl = OnObject.GetComponent<IngredientCtrl>();
        ctrl?.PoolingManager.Return(OnObject.gameObject);
        
        //OnObject.GetComponent<PlateCtrl>(); //.PoolingManager.Return(OnObject.gameObject)
    }
}
