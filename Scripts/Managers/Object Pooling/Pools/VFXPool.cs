using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPool : ObjectPoolBase
{
    protected override GameObject CreateAndEnqueueObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<VFXRemover>().SetParentPool(this);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }
}
