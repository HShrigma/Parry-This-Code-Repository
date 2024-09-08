using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : EnemyHitbox
{
    [Header("Gameplay")]
    [SerializeField] float colliderMaintainTime;
    [SerializeField] bool friendlyExplodeGrenades;
    float ignoreParryTime;
    bool ignoreParry;
    [Header("Physics")]
    [SerializeField] Collider coll;
    [SerializeField] protected LayerMask explosiveMask;
    [SerializeField] float cameraShakeAmount;

    protected void OnEnable()
    {
        ignoreParry = true;
        if (GameManager.instance != null)
        {
            GameManager.instance.GFXManager.ShakeCamGlobal(Vector3.one, cameraShakeAmount);
        }
        ignoreParryTime = colliderMaintainTime * 0.3f;
        colliderMaintainTime *= 0.7f;
        coll.enabled = true;
        StartCoroutine(DisableColliderAfterTime());
    }
    protected override void OnTriggerEnter(Collider other)
    {
        //handles player Collisions
        if (ignoreParry)
        {
            if (other.CompareTag("Player Hurtbox"))
            {
                PlayerController.instance.TakeDamage("explosion");
            }
        }
        else
        {
            HandlePlayerCollision(other);
        }
        HandleEnemyCollisions(other);
        HandleExplosiveCollisions(other);
        HandleDestructibleCollision(other);
    }

    protected void HandleEnemyCollisions(Collider collider)
    {
        if (collider.CompareTag("Enemy Hurtbox"))
        {
            collider.GetComponentInParent<Enemy>().TakeDamage("explosion");
        }
    }
    protected virtual void HandleExplosiveCollisions(Collider collider)
    {
        int layer = collider.gameObject.layer;
        if (layer == explosiveMask.value)
        {
            collider.GetComponent<Explosive>().Explode();
        }
    }
    IEnumerator DisableColliderAfterTime()
    {
        yield return new WaitForSeconds(ignoreParryTime);
        ignoreParry = false;
        yield return new WaitForSeconds(colliderMaintainTime);
        coll.enabled = false;
    }
}