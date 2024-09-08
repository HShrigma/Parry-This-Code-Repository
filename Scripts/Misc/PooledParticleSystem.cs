using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    ParticleEffectsPool parentPool;

    private void OnEnable()
    {
        if (particles != null)
        {
            particles.Play();
            StartCoroutine(ReturnAfterDonePlaying());
        }
        else
        {
            Debug.LogWarning($"Particle System {name} unassigned!");
        }
    }
    public void SetParentPool(ParticleEffectsPool parentPool)
    {
        this.parentPool = parentPool;
    }

    void ReturnToPool()
    {
        parentPool.ReturnToPool(gameObject);
    }

    IEnumerator ReturnAfterDonePlaying() 
    {
        yield return new WaitForSeconds(0.1f);
        while (particles.IsAlive(true)) 
        {
            yield return new WaitForSeconds(0.1f);
        }
        ReturnToPool();
    }
}
