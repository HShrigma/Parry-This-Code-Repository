using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    protected List<HitboxCollisionInfo> collisionBuffer = new List<HitboxCollisionInfo>();
    protected string dmgSource = "melee";
    protected virtual void FixedUpdate()
    {
        if (collisionBuffer.Count > 0)
        {
            if (collisionBuffer.Count == 1 && collisionBuffer[0].tag == "Player Hurtbox")
            {
                PlayerController.instance.TakeDamage(dmgSource);
            }
            DoAfterCollisionBufferClears();
            collisionBuffer.Clear();
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        HandlePlayerCollision(other);
        HandleDestructibleCollision(other);
    }
    protected bool ColliderIsPlayer(Collider collider)
    {
        return collider.CompareTag("Player Hitbox") || collider.CompareTag("Player Hurtbox");
    }
    protected virtual void DoAfterCollisionBufferClears()
    {

    }

    protected void HandlePlayerCollision(Collider other)
    {
        if (ColliderIsPlayer(other))
        {
            collisionBuffer.Add(new HitboxCollisionInfo(other.gameObject, other.tag));
        }
    }
    protected void HandleDestructibleCollision(Collider other)
    {
        if (other.CompareTag("Destructible"))
        {
            other.GetComponent<DestructibleObject>().TriggerDestruction();
        }
    }
}
