using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool : ObjectPoolBase
{
    protected override GameObject CreateAndEnqueueObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<Enemy>().SetParentPool(this);
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
            Enemy enemy = obj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetStats();
            }
            var agent = obj.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                agent.enabled = true;
            }
        }
        return obj;
    }
}
