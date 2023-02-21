using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemContainer : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public int existences;
    public int price;
    public Sprite sprite;

    public Image itemImage;

    void Start()
    {
        itemImage.sprite = sprite;
    }
}
