using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    public ShopExistences shopExistences;
    public IntValue redEggs;
    public GameObject shopPanel;
    int itemsCount = 0;
    List<GameObject> itemList = new();
    int currentItemIndex;

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescritionText;

    public GameObject itemBuyInfo;
    public TextMeshProUGUI existencesValue;
    public TextMeshProUGUI priceValue;
    public Button buyButton;

    // Start is called before the first frame update
    void Start()
    {
        DefaultShopInfo();
        for (int i = 0; i < shopPanel.transform.childCount; i++)
        {
            itemList.Add(shopPanel.transform.GetChild(i).gameObject);
            if (i > shopExistences.shopItems.Length - 1) itemList[i].SetActive(false);
            else itemsCount++;
        }
        ScaleShopPanel();
        AsignItemInfo();
    }

    void ScaleShopPanel()
    {
        RectTransform rect = shopPanel.GetComponent<RectTransform>();
        if (itemsCount > 4)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 80 + (200 * itemsCount) - 986.0f);
        }
        else
        {
            rect.sizeDelta = Vector2.zero;
        }
    }
    void AsignItemInfo()
    {
        for (int i = 0; i < shopExistences.shopItems.Length; i++)
        {
            ShopItemContainer aux = itemList[i].GetComponent<ShopItemContainer>();
            aux.itemName = shopExistences.shopItems[i].itemName;
            aux.itemDescription = shopExistences.shopItems[i].itemDescription;
            aux.existences = shopExistences.shopItemExistences[i];
            aux.price = shopExistences.shopItemPrice[i];
            aux.sprite = shopExistences.shopItems[i].itemSprite;
        }
    }

    public void SetItemInfo(int item)
    {
        ShopItemContainer aux = itemList[item].GetComponent<ShopItemContainer>();
        itemNameText.text = aux.itemName;
        itemDescritionText.text = aux.itemDescription;

        itemBuyInfo.SetActive(true);
        existencesValue.text = aux.existences.ToString();
        priceValue.text = aux.price.ToString();
        if (redEggs.value < aux.price) buyButton.interactable = false;
        else buyButton.interactable = true;
        if (aux.existences < 1) buyButton.gameObject.SetActive(false);
        else buyButton.gameObject.SetActive(true);

        currentItemIndex = item;
    }

    public void BuyItem()
    {
        redEggs.value -= shopExistences.shopItemPrice[currentItemIndex];
        shopExistences.shopItemPrice[currentItemIndex] += shopExistences.shopItemPriceIncrement[currentItemIndex];
        shopExistences.shopItemExistences[currentItemIndex]--;

        GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_Stats>().GetItem(shopExistences.shopItems[currentItemIndex].itemType);
        AsignItemInfo();
        SetItemInfo(currentItemIndex);
    }

    public void DefaultShopInfo()
    {
        itemNameText.text = "Sand Clock Shop";
        itemDescritionText.text = "Here you can buy some staff";
        itemBuyInfo.SetActive(false);
        currentItemIndex = -1;
    }
}
