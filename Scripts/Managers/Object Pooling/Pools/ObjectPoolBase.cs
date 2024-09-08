using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolBase : MonoBehaviour
{
    [Header("Pool Parameters")]
    //prefab for pool
    [SerializeField] protected GameObject prefab;
    //initial batch size
    [SerializeField] protected int initialPoolSize;
    //using queue as ordered pool items don't matter
    protected Queue<GameObject> pool;

    protected virtual void Awake()
    {
        InitializePool();
    }

    protected virtual void InitializePool()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateAndEnqueueObject();
        }
    }

    //create new object, sets it inactive, and adds it to the pool
    protected virtual GameObject CreateAndEnqueueObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    //get object from the pool
    public virtual GameObject GetGameObject()
    {
        if (pool.Count == 0)
        {
            #if UNITY_EDITOR
            Debug.Log($"{gameObject.name} pool is empty, expanding...");
            #endif
            CreateAndEnqueueObject();
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        return obj;
    }

    //get object with own rotation and set position
    public virtual GameObject GetGameObjectTransform(Vector3 position)
    {
        GameObject obj = GetGameObject();
        if (obj != null)
        {
            obj.transform.SetPositionAndRotation(position, obj.transform.rotation);
        }
        return obj;
    }

    //get object and set position and rotation
    public virtual GameObject GetGameObjectTransform(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetGameObject();
        if (obj != null)
        {
            obj.transform.SetPositionAndRotation(position, rotation);
        }
        return obj;
    }

    //get object and set as child of transform parent
    public virtual GameObject GetGameObjectTransform(Transform parent)
    {
        GameObject obj = GetGameObject();
        if (obj != null)
        {
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
        return obj;
    }

    //same as method above, but using position and rotation
    public virtual GameObject GetGameObjectTransform(Transform parent, Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetGameObject();
        if (obj != null)
        {
            obj.transform.SetParent(parent);
            obj.transform.SetPositionAndRotation(position, rotation);
        }
        return obj;
    }

    //return object to pool
    public virtual void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}