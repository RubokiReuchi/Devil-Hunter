using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandClock : MonoBehaviour, DataPersistenceInterfice
{
    public ShopExistences shopExistences;

    public void LoadData(GameData data)
    {
        shopExistences.shopItemExistences = data.sandClockExistences;
        shopExistences.shopItemPrice = data.sandClockPrice;
    }

    public void SaveData(GameData data)
    {
        data.sandClockExistences = shopExistences.shopItemExistences;
        data.sandClockPrice = shopExistences.shopItemPrice;
    }
}
