using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float max_hp;
    public float attack;
    public float regen_hp;
    public float life_steal;
    public float knockback_resist;
    [NonEditable] public float current_hp;

    protected bool update;
    public GameObject floating_text;

    // Start is called before the first frame update
    void Start()
    {
        current_hp = max_hp;
        update = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            StartCoroutine("EachSecond");
        }
    }

    public void Heal(float amount)
    {
        current_hp += amount;
        if (current_hp > max_hp)
        {
            amount -= current_hp - max_hp;
            current_hp = max_hp;

        }

        if (floating_text)
        {
            Vector2 circle = Random.insideUnitCircle * 0.75f;
            GameObject text = Instantiate(floating_text, transform.position + new Vector3(circle.x, circle.y, 0), Quaternion.identity);

            float rounded = Mathf.Round(amount * 100.0f) / 100.0f;
            TextMesh aux = text.GetComponentInChildren<TextMesh>();
            aux.text = rounded.ToString();

            aux.color = Color.green;
        }
    }

    public void LifeSteal(float damage)
    {
        if (life_steal > 0 && current_hp < max_hp)
        {
            Heal(damage * (life_steal / 100.0f));
        }
    }

    protected IEnumerator EachSecond()
    {
        update = false;
        yield return new WaitForSeconds(1);
        update = true;

        if (regen_hp > 0 && current_hp < max_hp)
        {
            Heal(regen_hp);
        }
    }
}
