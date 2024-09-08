using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPoolsManager : MonoBehaviour
{
    [Header("Dash VFX")]
    public VFXPool PlayerDash1Pool;
    public VFXPool PlayerDash2Pool;
    [Header("Smoke VFX")]
    [SerializeField] List<VFXPool> smokePools;
    [SerializeField] VFXPool defaultSmoke;
    [Header("Parry VFX")]
    [SerializeField] List<VFXPool> parryPools;
    [Header("Bullet VFX")]
    public VFXPool BulletHitPool;
    public VFXPool BulletWallPool;
    [Header("Explosion VFX")]
    public VFXPool ExplosionPool;
    public VFXPool FriendlyExplosionPool;
    public VFXPool BazookaExplosionPool;
    public VFXPool BazookaFriendlyExplosionPool;
    [Header("Ranged VFX")]
    public VFXPool MuzzleFlashPool;
    public VFXPool GrenadeLaunchPool;
    public VFXPool RocketLaunchPool;

    List<string> keys = new List<string>()
    {
        "melee","eliteMelee",
        "gun","eliteGun",
        "uzi","eliteUzi",
        "grenade","eliteGrenade",
        "bazooka"
    };

    public VFXPool GetSmokePoolForKey(string key)
    {
        int index = keys.IndexOf(key);
        if (index >= 0 && index < smokePools.Count)
        {
            return smokePools[index];
        }
        else
        {
            Debug.LogWarning($"Smoke pool for key '{key}' not found.");
            return defaultSmoke;
        }
    }

    public VFXPool GetParryPoolForIndex(int index)
    {
        index = Mathf.Clamp(index, 0, parryPools.Count);
        return parryPools[index];
    }
}
