using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltLightnings : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> particles = new();
    [HideInInspector] public Hitbox objectiveHitbox;
    public float damagePerLighning;
    GameObject objective;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        if (objectiveHitbox == null) return;

        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            objectiveHitbox.TakeDamage(damagePerLighning, objectiveHitbox.transform.position);
            particles[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }

    public void SetUltObjective(GameObject objective)
    {
        this.objective = objective;
    }

    public IEnumerator PlayUlt()
    {
        ps.Play();
        yield return new WaitForSeconds(1);
        ps.externalForces.AddInfluence(objective.GetComponentInChildren<ParticleSystemForceField>());
        ps.trigger.AddCollider(objective.GetComponent<Collider2D>());
        objectiveHitbox = objective.GetComponent<Hitbox>();
        yield return new WaitForSeconds(4);
        objective = null;
        ps.externalForces.RemoveInfluence(0);
        ps.trigger.RemoveCollider(0);
        objectiveHitbox = null;
    }
}
