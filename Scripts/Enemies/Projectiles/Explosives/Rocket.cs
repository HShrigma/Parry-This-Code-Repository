using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.VFX;

public class Rocket : Explosive
{
    enum rocketStates
    {
        moving,
        warning,
        exploding,
        parriable,
        parried,
        exploded
    }
    rocketStates state;
    [Header("Physics")]
    [SerializeField] float speed = 10f;
    [SerializeField] float torqueForce = 1f;
    [SerializeField] float warningDist;
    [SerializeField] float explosionDist;
    [SerializeField] float explosionTime;
    [SerializeField] float explosionDelay;
    PlayerController player;
    [SerializeField] Rigidbody rb;
    Quaternion lastRotation;
    float lastYrotation;
    [Header("Prefabs")]
    VFXPool explosionPool;
    VFXPool friendlyExplosionPool;
    [Header("VFX")]
    [SerializeField] VisualEffect propellerVFX;
    [SerializeField] VisualEffect smokeVFX;
    [SerializeField] List<VisualEffect> greenBlinkers;
    [SerializeField] List<VisualEffect> redBlinkers;
    [SerializeField] VisualEffect explosionChargeVFX;
    VFXPool parryPool;
    [SerializeField] float smokeRepeatTime;
    [SerializeField] float blinkerCycleTime;
    [SerializeField] float blinkerRestTime;
    [Header("Sound")]
    [SerializeField] float warningSoundBaseRepeat;
    int beepIndex;
    int beepCount;
    int loopIndex;
    float loopSoundTime;
    ProjectileSFXManager soundFX;
    [SerializeField] AudioSource loopSRC;
    [SerializeField] List<AudioClip> loopClips;

    public override void SetCommands()
    {
        state = rocketStates.moving;
        soundFX = GameManager.instance.ProjectileSoundFXManager;
        beepIndex = 0;
        beepCount = soundFX.GetBeepsCount();
        player = PlayerController.instance;
        lastRotation = rb.rotation;
        lastYrotation = transform.eulerAngles.y;
        VFXPoolsManager poolManager = ObjectPoolManager.instance.VFXPoolsManager;
        parryPool = poolManager.GetParryPoolForIndex(3);
        explosionPool = poolManager.BazookaExplosionPool;
        friendlyExplosionPool = poolManager.BazookaFriendlyExplosionPool;
        StartCoroutine(PlaySmokeVFXRecursively());
        StartCoroutine(PlayBlinkersRecursively());
        PlayLoopSound();
    }
    void FixedUpdate()
    {
        //update RB forces and rotation
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        RotateTowardsDir();
    }

    void Update()
    {
        // Calculate the rotation difference
        float rotationDifference = Quaternion.Angle(lastRotation, rb.rotation);

        // Normalize and map rotation difference to -1 to 1
        float normalizedValue = Mathf.Clamp(rotationDifference, 0f, 1f); // Adjust divisor as needed
        if (transform.eulerAngles.y < lastYrotation)
        {
            normalizedValue *= -1f;
        }
        SetState();
        // Set the VFX parameter
        propellerVFX.SetFloat("FireXForce", normalizedValue);
        // Update the previous rotation for the next frame
        lastYrotation = transform.eulerAngles.y;
        lastRotation = rb.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        //explode on collision with terrain,small terrain, ground, default
        if (!other.CompareTag("Player Hitbox") && !other.CompareTag("Player Hurtbox") && other.gameObject.layer != 10)
        {
            Explode();
        }
    }
    void SetState()
    {
        float distToPlayer = GetDistToPlayer();
        if (distToPlayer <= warningDist && state == rocketStates.moving)
        {
            SetWarning();
        }
        if (distToPlayer <= explosionDist && state == rocketStates.warning)
        {
            SetExploding();
        }
    }
    float GetDistToPlayer()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    void SetWarning()
    {
        state = rocketStates.warning;
        StartCoroutine(PlayWarningSoundRecursive());
    }
    void SetExploding()
    {
        state = rocketStates.exploding;
        StartCoroutine(ExplodeAfterTime());
    }
    //Gets player x,z and own Y
    Vector3 GetLookPos()
    {
        return new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
    Vector3 GetLookDir()
    {
        Vector3 dir = GetLookPos() - transform.position;
        dir.y = 0f;
        return dir;
    }
    void RotateTowardsDir()
    {
        // Calculate the desired rotation to face the direction
        Vector3 directionToTarget = GetLookDir();
        directionToTarget.y = 0f; // Ignore vertical direction

        if (directionToTarget != Vector3.zero) // Ensure direction is not zero
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion currentRotation = rb.rotation;
            Quaternion desiredRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * torqueForce);

            rb.MoveRotation(desiredRotation); // Apply the rotation
        }
    }

    IEnumerator PlaySmokeVFXRecursively()
    {
        yield return new WaitForSeconds(smokeRepeatTime);
        SetSmokeTransform();
        smokeVFX.Play();
        StartCoroutine(PlaySmokeVFXRecursively());
    }
    void SetSmokeTransform()
    {
        //set custom position and rotation as smoke VFX uses world position
        Vector3 pos = smokeVFX.transform.position;
        smokeVFX.SetVector3("SpawnPosition", pos);
        smokeVFX.SetVector3("SpawnAngle", transform.rotation.eulerAngles);
    }

    IEnumerator PlayBlinkersRecursively()
    {
        //wait the rest time
        yield return new WaitForSeconds(blinkerRestTime);
        //Play each green blinker
        foreach (VisualEffect blinker in greenBlinkers)
        {
            yield return new WaitForSeconds(blinkerCycleTime);
            blinker.Play();
        }
        //wait the rest time
        yield return new WaitForSeconds(blinkerRestTime);
        //Play each red blinker
        foreach (VisualEffect blinker in redBlinkers)
        {
            yield return new WaitForSeconds(blinkerCycleTime);
            blinker.Play();
        }
        StartCoroutine(PlayBlinkersRecursively());
    }
    void PlayLoopSound()
    {
        //Select loop based on index
        loopIndex = Random.Range(0, loopClips.Count);
        loopSRC.clip = loopClips[loopIndex];
        loopSRC.Play();
    }
    IEnumerator PlayWarningSoundRecursive()
    {
        float baseDiff = warningDist - explosionDist;
        float repeatTime = Mathf.Lerp(0.1f, warningSoundBaseRepeat, (GetDistToPlayer() - explosionDist) / baseDiff);

        yield return new WaitForSeconds(repeatTime);
        soundFX.PlayBazookaBeep(beepIndex);
        IncrementWarningSoundIndex();
        if (state == rocketStates.warning)
        {
            StartCoroutine(PlayWarningSoundRecursive());
        }
    }

    void IncrementWarningSoundIndex()
    {
        beepIndex++;
        beepIndex %= beepCount;
    }
    public override void Explode()
    {
        if (state != rocketStates.exploded)
        {
            state = rocketStates.exploded;
            StopAllCoroutines();
            loopSRC.Stop();
            soundFX.PlayBazookaExplode();
            explosionPool.GetGameObjectTransform(transform.position,Quaternion.identity);
            ReturnToPool();
        }
    }

    public override void ExplodeFriendly()
    {
        if (state != rocketStates.exploded)
        {
            state = rocketStates.exploded;
            StopAllCoroutines();
            loopSRC.Stop();
            soundFX.PlayBazookaExplode();
            GameManager.instance.IncrementScore();
            friendlyExplosionPool.GetGameObjectTransform(transform.position, Quaternion.identity);
            ReturnToPool();
        }
    }

    IEnumerator ExplodeAfterTime()
    {
        StopCoroutine(PlayWarningSoundRecursive());
        soundFX.PlayBazookaLastBeep();
        explosionChargeVFX.SetFloat("GlobalLifetime", explosionTime);
        explosionChargeVFX.SendEvent("CustomPlay");
        yield return new WaitForSeconds(explosionDelay);
        state = rocketStates.parriable;
        yield return new WaitForSeconds(explosionTime);
        if (state != rocketStates.parried && state != rocketStates.exploded)
        {
            Explode();
        }
    }
    public override void HandlePlayerHit()
    {
        if (state == rocketStates.parriable)
        {
            state = rocketStates.parried;

            parryPool.GetGameObjectTransform(transform.position, transform.rotation);
            player.SetParryBox();
            ExplodeFriendly();
        }
        else
        {
            Debug.Log("Not parriable, exploding");
            player.TakeDamage("explosion");
            Explode();
        }
    }
}
