using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : ObjectPoolBase
{
    protected override GameObject CreateAndEnqueueObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<Bullet>().SetParentPool(this);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }
    public override GameObject GetGameObjectTransform(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetGameObject();
        if (obj != null)
        {
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.GetComponent<Bullet>().SetCommands();
        }
        return obj;
    }
}
