using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Vector3 spawn;
    protected Animator anim;
    public float speed;
    public float attack_cd;
    [HideInInspector] public float orientation;

    // Check lateral
    [Header("Check Lateral")]
    public Vector2 box_size_lateral;
    public float max_distance_left;
    public float max_distance_right;
    public float max_distance_up;
    public LayerMask layer_mask_lateral;

    // death
    [Header("On Death")]
    protected SpriteRenderer sprite;
    protected float sprite_alpha;
    protected bool death;
    public GameObject dropRedEggs;
    public int eggsAmount;

    // restrict movement
    [Header("Restrict Movement")]
    [NonEditable] public bool restrict_left;
    [NonEditable] public bool restrict_right;

    public void AttackMelee()
    {
        anim.SetBool("CanAttack", false);
    }

    public IEnumerator Co_Attack()
    {
        yield return new WaitForSeconds(attack_cd);
        anim.SetBool("CanAttack", true);
    }

    public IEnumerator Death()
    {
        death = true;
        ParticleSystem dust = GetComponentInChildren<ParticleSystem>();
        var aux = dust.shape;
        aux.scale = new Vector3(transform.localScale.x, 1, 1);
        dust.Play();
        GameObject drop = Instantiate(dropRedEggs, transform.position, Quaternion.identity);
        drop.GetComponent<DropRedEggs>().particleAmount = eggsAmount;
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.right * max_distance_left * transform.localScale.x + transform.up * max_distance_up, box_size_lateral);
        Gizmos.DrawCube(transform.position + transform.right * max_distance_right * transform.localScale.x + transform.up * max_distance_up, box_size_lateral);
    }
}
