using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "new shopExistence")]
public class ShopExistences : ScriptableObject
{
    public ShopItem[] shopItems;
    public int[] shopItemExistences;
    public int[] shopItemPrice;
    public int[] shopItemPriceIncrement;
}
