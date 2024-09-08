using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyExplosion : Explosion
{ 
    protected override void OnTriggerEnter(Collider other)
    {
        HandleEnemyCollisions(other);
        HandleDestructibleCollision(other);
        HandleExplosiveCollisions(other);
    }
    protected override void HandleExplosiveCollisions(Collider collider)
    {
        int layer = collider.gameObject.layer;
        if (layer == explosiveMask.value)
        {
            collider.GetComponent<Explosive>().ExplodeFriendly();
        }
    }
}
