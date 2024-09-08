using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosive : MonoBehaviour
{
    protected ExplosivePool parentPool;
    public abstract void HandlePlayerHit();

    public abstract void Explode();
    public abstract void ExplodeFriendly();

    public void SetParentPool(ExplosivePool parentPool)
    {
        this.parentPool = parentPool;
    }

    protected void ReturnToPool()
    {
        parentPool.ReturnToPool(gameObject);
    }

    public abstract void SetCommands();
}
