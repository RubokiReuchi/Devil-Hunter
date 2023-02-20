using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropRedEggs : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> particles = new();
    public int particleAmount;

    public IntValue redEggs;
    
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.Burst burst = ps.emission.GetBurst(0);
        burst.count = particleAmount;
        ps.emission.SetBurst(0, burst);
        ps.Play();
        StartCoroutine("LifeTime");
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1.0f);
        if (GameObject.FindGameObjectWithTag("Dante") != null)
        {
            ps.externalForces.SetInfluence(0, GameObject.FindGameObjectWithTag("Dante").GetComponentInChildren<ParticleSystemForceField>());
            ps.trigger.SetCollider(0, GameObject.FindGameObjectWithTag("Dante").GetComponent<Collider2D>());
        }
        yield return new WaitForSeconds(9.0f);
        Destroy(gameObject);
    }

    private void OnParticleTrigger()
    {
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            redEggs.value += (int)(p.startSize * 10.0f);
            particles[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }
}
