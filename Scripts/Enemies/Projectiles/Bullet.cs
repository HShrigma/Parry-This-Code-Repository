using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : EnemyHitbox
{
    protected BulletPool parentPool;
    [Header("Physics")]
    [SerializeField] Rigidbody rb;
    PlayerController player;
    [SerializeField] float force;
    [SerializeField] float recoil;
    [SerializeField][Range(0f, 1f)] float maxOffset;
    ProjectileSFXManager soundFX;
    [Header("VFX")]
    VFXPool parryPool;
    VFXPool hitPool;
    VFXPool wallPool;
    void GetDirToPlayer()
    {
        transform.rotation = Quaternion.LookRotation(ProjectileHelper.SetDirTowardsPlayer(player.transform, transform, maxOffset, recoil));
    }
    protected override void FixedUpdate()
    {
        if (collisionBuffer.Count > 0)
        {
            if (collisionBuffer.Count == 1 && collisionBuffer[0].tag == "Player Hurtbox")
            {
                PlayerController.instance.TakeDamage(dmgSource);
                Kill();
            }
            collisionBuffer.Clear();
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.layer == 11)
        {
            HitWall();
        }
    }

    public void Parry()
    {
        GameManager.instance.IncrementScore();
        soundFX.PlayBulletParry();
        parryPool.GetGameObjectTransform(transform.position, transform.rotation);
        parentPool.ReturnToPool(gameObject);
    }

    void HitWall()
    {
        soundFX.PlayBulletRicochet();
        wallPool.GetGameObjectTransform(transform.position, transform.rotation);
        parentPool.ReturnToPool(gameObject);
    }
    void Kill()
    {
        soundFX.PlayBulletHit();
        hitPool.GetGameObjectTransform(transform.position, transform.rotation);
        parentPool.ReturnToPool(gameObject);
    }
    protected override void DoAfterCollisionBufferClears()
    {
        Kill();
    }

    public void SetParentPool(BulletPool parentPool)
    {
        this.parentPool = parentPool;
    }
    public void SetCommands()
    {
        dmgSource = "ranged";

        if (GameManager.instance != null && soundFX == null)
        {
            soundFX = GameManager.instance.ProjectileSoundFXManager;
        }
        if (CamFadeDetect.instance != null)
        {
            CamFadeDetect.instance.AddProjectile(gameObject);
        }
        if (ObjectPoolManager.instance != null && (parryPool == null || hitPool == null || wallPool == null ))
        {
            VFXPoolsManager vFXPools = ObjectPoolManager.instance.VFXPoolsManager;
            parryPool = vFXPools.GetParryPoolForIndex(1);
            hitPool = vFXPools.BulletHitPool;
            wallPool = vFXPools.BulletWallPool;
        }
        if (PlayerController.instance != null)
        {
            player = PlayerController.instance;
            rb.WakeUp();
            GetDirToPlayer();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = transform.rotation;
            rb.position = transform.position;
            rb.AddForce(transform.forward * force,ForceMode.VelocityChange);
        }
    }
}
