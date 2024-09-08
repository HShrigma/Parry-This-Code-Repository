using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsPool : ObjectPoolBase
{
    protected override GameObject CreateAndEnqueueObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<PooledParticleSystem>().SetParentPool(this);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }
}
