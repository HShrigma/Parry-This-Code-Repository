using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolsManager : MonoBehaviour
{
    [Header("Bullets")]
    public BulletPool BulletPool;
    public BulletPool UziBulletPool;
    [Header("Explosives")]
    public ExplosivePool GrenadePool;
    public ExplosivePool RocketPool;

    public ObjectPoolBase GetPoolForEnemyType(string key)
    {
        switch (key)
        {
            default:
                return BulletPool;
            case "uzi":
                return UziBulletPool;
            case "grenade":
                return GrenadePool;
            case "rocket":
                return RocketPool;
        }
    }
}
