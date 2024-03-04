using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSheetCtrl : MonoBehaviour
{
    [SerializeField]
    Image _menuImage;

    public void SetMenuImage(Sprite menuSprite) { _menuImage.sprite = menuSprite; }
}
