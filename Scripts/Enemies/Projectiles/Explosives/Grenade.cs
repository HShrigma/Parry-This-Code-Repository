using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Grenade : Explosive
{
    enum grenadeState
    {
        unparriable,
        parriable,
        parried,
        exploding,
        exploded
    }
    grenadeState state;
    [Header("Physics")]
    [SerializeField] float forwardForce;
    [SerializeField] float upwardForce;
    [SerializeField][Range(0f, 1f)] float maxOffset;
    [SerializeField] float recoil;
    [SerializeField] LayerMask environmentLayers;
    [SerializeField] Rigidbody rb;
    PlayerController player;
    [Header("Time-Based")]
    [SerializeField] float parryTransitonTime;
    [SerializeField] float parryStayTime;
    [SerializeField] float baseStayTime;
    [SerializeField] float explosionTime;
    [SerializeField] float explosionParryTime;
    [Header("VFX")]
    [SerializeField][ColorUsage(true, true)] Color baseColor;
    [SerializeField][ColorUsage(true, true)] Color parryColor;
    VFXPool parryPool;
    VFXPool explosionPool;
    VFXPool friendlyExplosionPool;
    [SerializeField] VisualEffect glow;
    [SerializeField] VisualEffect smokeVFX;
    [SerializeField] float smokeSpawnTime;
    [SerializeField] float explosionGlobalSize;
    [SerializeField] float defaultGlobalSize;
    ProjectileSFXManager soundFXManager;
    public override void SetCommands()
    {
        soundFXManager = GameManager.instance.ProjectileSoundFXManager;
        VFXPoolsManager vFXPools = ObjectPoolManager.instance.VFXPoolsManager;
        parryPool = vFXPools.GetParryPoolForIndex(2);
        explosionPool = vFXPools.ExplosionPool;
        friendlyExplosionPool = vFXPools.FriendlyExplosionPool;
        player = PlayerController.instance;

        glow.SetFloat("GlobalSize", defaultGlobalSize);

        state = grenadeState.parriable;

        ResetRB();
        AddLaunchForce();
        StartCoroutine(PlaySmokeRecursive());
        StartCoroutine(TransitionColorsRecursive());
        StartCoroutine(ExplodeAfterTime(explosionTime, explosionParryTime));
    }
    private void OnCollisionEnter(Collision collision)
    {
        //perform bitwise AND to check if layermask is within environment layers
        if ((environmentLayers & (1 << collision.gameObject.layer)) > 0)
        {
            soundFXManager.PlayGrenadeBounce();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy Hurtbox") && state == grenadeState.parried) 
        {
            ExplodeFriendly();
        }
    }
    void LookAtPlayerWithinBounds()
    {
        transform.rotation = Quaternion.LookRotation(ProjectileHelper.SetDirTowardsPlayer(player.transform, transform, maxOffset, recoil));
    }
    Vector3 GetLaunchDirTimesForces()
    {
        LookAtPlayerWithinBounds();
        Vector3 dir = new Vector3(transform.forward.x, transform.up.y, transform.forward.z).normalized;
        return new Vector3(dir.x * forwardForce, dir.y * upwardForce, dir.z * forwardForce);
    }
    void AddLaunchForce()
    {
        rb.AddForce(GetLaunchDirTimesForces(), ForceMode.VelocityChange);
    }

    IEnumerator PlaySmokeRecursive()
    {
        smokeVFX.SetVector3("SpawnPosition", transform.position);
        smokeVFX.Play();
        yield return new WaitForSeconds(smokeSpawnTime);
        StartCoroutine(PlaySmokeRecursive());
    }

    void TransitionToBase(float progress)
    {
        LerpColourForTimeStep(parryColor, baseColor, progress);
    }
    void TransitionToParry(float progress)
    {
        LerpColourForTimeStep(baseColor, parryColor, progress);
    }
    IEnumerator TransitionColorsRecursive()
    {
        glow.SetVector4("Color", parryColor);
        yield return new WaitForSeconds(parryStayTime);

        float progress = 0f;

        while (!IsGlowColor(baseColor))
        {
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime / parryTransitonTime;
            TransitionToBase(progress);
        }
        if (state != grenadeState.parried)
        {
            state = grenadeState.unparriable;
        }

        yield return new WaitForSeconds(baseStayTime);

        progress = 0f;

        while (!IsGlowColor(parryColor))
        {
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime / parryTransitonTime;
            TransitionToParry(progress);
        }
        if (state != grenadeState.parried)
        {
            state = grenadeState.parriable;
        }
        StartCoroutine(TransitionColorsRecursive());
    }

    bool IsGlowColor(Color toCompare)
    {
        return (Color)glow.GetVector4("Color") == toCompare;
    }
    void LerpColourForTimeStep(Color from, Color to, float progress)
    {
        Color newClr = Color.Lerp(from, to, progress);
        glow.SetVector4("Color", newClr);
    }

    IEnumerator ExplodeAfterTime(float explosionTime, float explosionParryTime)
    {
        yield return new WaitForSeconds(explosionTime);
        StopCoroutine(TransitionColorsRecursive());
        if (state != grenadeState.parried)
        {
            state = grenadeState.exploding;
        }
        glow.SetVector4("Color", parryColor);
        glow.SetFloat("ChargeCount", 0f);
        float progress = 0f;
        float initSize = glow.GetFloat("GlobalSize");
        while (glow.GetFloat("GlobalSize") != explosionGlobalSize)
        {
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime / explosionParryTime;
            glow.SetFloat("GlobalSize", Mathf.Lerp(initSize, explosionGlobalSize, progress));
        }
        if (state != grenadeState.parried)
        {
            Explode();
        }
        else
        {
            ExplodeFriendly();
        }
    }
    public override void HandlePlayerHit()
    {
        switch (state)
        {
            case grenadeState.unparriable:
                PlayerController.instance.TakeDamage("explosion");
                Explode();
                break;
            case grenadeState.parriable:
                GetParried();
                break;
            case grenadeState.exploding:
                ExplodeFriendly();
                break;
        }
    }

    public override void Explode()
    {
        if (state != grenadeState.exploded)
        {
            state = grenadeState.exploded;
            StopAllCoroutines();
            soundFXManager.PlayGrenadeExplode();
            explosionPool.GetGameObjectTransform(transform.position, Quaternion.identity);
            ReturnToPool();
        }
    }
    public override void ExplodeFriendly()
    {
        if (state != grenadeState.exploded)
        {
            state = grenadeState.exploded;
            GameManager.instance.IncrementScore();
            StopAllCoroutines();
            soundFXManager.PlayGrenadeExplode();
            friendlyExplosionPool.GetGameObjectTransform(transform.position, Quaternion.identity);
            ReturnToPool();
        }

    }
    void GetParried()
    {
        StopAllCoroutines();
        state = grenadeState.parried;
        soundFXManager.PlayGrenadeParry();
        float newExplosionTime = 0.01f;
        float newExplosionParryTime = 0.3f;
        LaunchFromPlayer();
        player.SetParryBox();
        parryPool.GetGameObjectTransform(transform.position, transform.rotation);

        StartCoroutine(ExplodeAfterTime(newExplosionTime, newExplosionParryTime));
    }

    void LaunchFromPlayer()
    {

        // Reflect the direction vector around the player's forward direction
        Vector3 reflectedDir = EnemyStateHelper.GetDiryFromXtoY(transform, player.transform) * -1f;

        // Normalize and adjust the direction
        reflectedDir = new Vector3(reflectedDir.x, 0f, reflectedDir.z).normalized;
        ResetRB();
        // Apply force in the reflected direction
        rb.AddForce(reflectedDir * forwardForce * 2f, ForceMode.VelocityChange);
    }
    void ResetRB()
    {
        // Reset velocities before applying new force
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.freezeRotation = true;
    }
}
