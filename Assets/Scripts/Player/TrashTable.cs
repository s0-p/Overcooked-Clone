using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TrashTable : BaseTable
{
    public override void PutOnObject(Transform objectTransform)
    {
        base.PutOnObject(objectTransform);

        StartCoroutine(CRT_ThrowAway());
    }
    IEnumerator CRT_ThrowAway()
    {
        enabled = false;

        while (OnObject != null && OnObject.transform.localScale.x > 0.1)
        {
            OnObject.transform.localScale *= 0.8f;
            OnObject.transform.Rotate(Vector3.up * 30f);
            yield return new WaitForSeconds(0.1f);
        }
        if (OnObject != null)
            Destroy(OnObject.gameObject);
        
        enabled = true;
    }
}
