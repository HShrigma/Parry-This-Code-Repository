using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    [Header("VFX Pools")]
    public VFXPoolsManager VFXPoolsManager;
    [Header("ParticlePools")]
    public ParticleEffectsPoolManager particleEffectsPoolManager;
    [Header("Projectile Pools")]
    public ProjectilePoolsManager ProjectilePoolsManager;
    [Header("Enemy Pools")]
    public EnemyPoolsManager EnemyPools;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}