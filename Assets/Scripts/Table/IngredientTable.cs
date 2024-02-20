using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientTable : TableBase
{
    [SerializeField]
    GameObject _ingredientPrefab;

    public override void Operate(GameObject player)
    {
        if (_ingredientPrefab != null)
        {
            GameObject ingredient = Instantiate(
                        _ingredientPrefab,
                        transform.position + Vector3.up,
                        Quaternion.identity);

            ingredient.GetComponent<Rigidbody>().isKinematic = true;

            player.GetComponent<PlayerMove>().enabled = false;
            player.GetComponent<PlayerAnimation>().PickUpAni();
        }
    }
}
