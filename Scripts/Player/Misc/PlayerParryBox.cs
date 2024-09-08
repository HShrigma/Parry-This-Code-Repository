using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerParryBox : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
    [SerializeField] Collider coll;
    [SerializeField] float maintainTime;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy Hitbox":
                Enemy enemy = other.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    enemy.SetState(Enemy.enemyStates.Parried);
                }
                break;
            case "Projectile":
                other.GetComponent<Bullet>().Parry();
                break;
            case "Explosive":
                other.GetComponent<Explosive>().ExplodeFriendly();
                break;
            case "Destructible":
                other.GetComponent<DestructibleObject>().TriggerDestruction();
                break;
        }
    }
    private void Start()
    {
        coll.enabled = false;
        vfx.SetFloat("LifetimeGlobal", maintainTime);
    }
    void PlayVFX()
    {
        vfx.SendEvent("CustomPlay");
    }
    public void EnableForTime()
    {
        coll.enabled = true;
        PlayVFX();
        StartCoroutine(DisableAfterTime());
    }
    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(maintainTime);
        coll.enabled = false;
    }
    public bool IsColliderActive()
    {
        return coll.enabled;
    }
}
