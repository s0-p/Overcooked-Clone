using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCtrl : MonoBehaviour
{
    [SerializeField]
    float _spawnDeley = 3;
    //--------------------------------------------------------------
    public Vector3 SpawnPosition { get; set; }
    //--------------------------------------------------------------
    int _includedIngredientsBit;
    public int IncludedIngredientsBit { get { return _includedIngredientsBit;} }
    //--------------------------------------------------------------
    private void OnEnable()
    {
        _includedIngredientsBit = 0;
    }
    void OnDisable()
    {
        Invoke("Spawn", _spawnDeley);
    }
    public void OnDisableCustom()
    {
        for (int index = 0; index < transform.childCount; index++)
        {
            Transform child = transform.GetChild(index);
            child.gameObject.SetActive(true);

            child.GetComponent<Rigidbody>().isKinematic = false;
            child.GetComponentInChildren<Collider>().enabled = true;
            child.parent = null;

            child.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    void Spawn()
    {
        transform.position = SpawnPosition;
        gameObject.SetActive(true);
    }
    //--------------------------------------------------------------
    void OnCollisionEnter(Collision collision)
    {
        IngredientCtrl ingredientCtrl = collision.transform.GetComponent<IngredientCtrl>();
        if (ingredientCtrl != null && ingredientCtrl.IsCooked)
        {
            Debug.Log("재료 넣기!");
            _includedIngredientsBit += ingredientCtrl.BitId;

            collision.transform.parent = transform;
            collision.transform.SetPositionAndRotation(
                        transform.position + Vector3.up * 0.1f,
                        Quaternion.identity
                    );
            
            collision.transform.GetComponent<Rigidbody>().isKinematic = true;
            collision.transform.GetComponentInChildren<Collider>().enabled = false;
            collision.gameObject.layer = 0;
        }
    }
}
