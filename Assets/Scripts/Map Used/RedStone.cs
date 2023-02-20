using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStone : MonoBehaviour, DataPersistenceInterfice
{
    [SerializeField] string id;
    [ContextMenu("Genetate guid for id")]
    void GenerateUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    Animator anim;
    public GameObject dropRedEggs;
    public int eggsAmount;
    bool broken;

    public void LoadData(GameData data)
    {
        data.redStonesPicked.TryGetValue(id, out broken);
        if (broken) gameObject.SetActive(false);
    }

    public void SaveData(ref GameData data)
    {
        if (data.redStonesPicked.ContainsKey(id)) data.redStonesPicked.Remove(id);
        data.redStonesPicked.Add(id, broken);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DanteSword"))
        {
            anim.SetTrigger("Destroy");
            StartCoroutine("Co_Destroy");
        }
    }

    IEnumerator Co_Destroy()
    {
        broken = true;
        GameObject drop = Instantiate(dropRedEggs, transform.position, Quaternion.identity);
        drop.GetComponent<DropRedEggs>().particleAmount = eggsAmount;
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }
}