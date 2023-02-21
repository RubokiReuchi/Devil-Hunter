using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new shopItem")]
public class ShopItem : ScriptableObject
{
    public Sprite itemSprite;
    public ITEM itemType;
    public string itemName;
    public string itemDescription;
}
