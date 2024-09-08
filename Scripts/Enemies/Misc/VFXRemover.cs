using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXRemover: MonoBehaviour
{
    public VFXPool ParentPool {  get; private set; }
    [SerializeField] VisualEffect effect;
    void OnEnable()
    {
        if(ParentPool != null)
        {
            StartCoroutine(CheckForParticleCompletion());
        }
    }

    IEnumerator CheckForParticleCompletion()
    {
        yield return new WaitForSeconds(0.1f);
        while (effect.aliveParticleCount > 0)
        {
            yield return new WaitForSeconds(0.1f); // Check every 100ms instead of every frame
        }
        ParentPool.ReturnToPool(gameObject);
    }
    public void SetParentPool(VFXPool pool)
    {
        ParentPool = pool;
    }
}
