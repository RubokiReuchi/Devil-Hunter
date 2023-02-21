using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

public class ItemOnFloor : MonoBehaviour, DataPersistenceInterfice
{
    [SerializeField] string id;
    [ContextMenu("Genetate guid for id")]
    void GenerateUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public Sprite blueEggSprite;
    public Sprite purpleEggSprite;
    public Sprite goldEggSprite;
    SpriteRenderer sprite;
    public ITEM itemType;
    bool picked;

    public void LoadData(GameData data)
    {
        data.itemOnFloorPicked.TryGetValue(id, out picked);
        if (picked) gameObject.SetActive(false);
    }

    public void SaveData(GameData data)
    {
        if (data.itemOnFloorPicked.ContainsKey(id)) data.itemOnFloorPicked.Remove(id);
        data.itemOnFloorPicked.Add(id, picked);
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        switch (itemType)
        {
            case ITEM.BLUE_EGGS_FRAGMENT:
                sprite.sprite = blueEggSprite;
                break;
            case ITEM.PURPLE_EGGS_FRAGMENT:
                sprite.sprite = purpleEggSprite;
                break;
            case ITEM.GOLD_EGGS_FRAGMENT:
                sprite.sprite = goldEggSprite;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dante"))
        {
            picked = true;
            collision.GetComponent<Dante_Stats>().GetItem(itemType);
            // open popup
            gameObject.SetActive(false);
        }
    }
}
