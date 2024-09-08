using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyArmourManager : MonoBehaviour
{
    [Header("Armour pieces")]
    [SerializeField] List<GameObject> armourPieces = new List<GameObject>();
    [Header("Smoke Pool Reference")]
    [SerializeField] string smokePoolKey;
    VFXPool smokePool;
    ParticleEffectsPoolManager particleEffectsPoolManager;
    [Header("Camera Shake Parameters")]
    [SerializeField] float shakeCamMultHit;
    [SerializeField] float shakeCamMultDead;
    [Header("References")]
    [SerializeField] CameraShaker camShaker;
    [SerializeField] Enemy self;

    ParticleEffectsPool SideParticles;
    ParticleEffectsPool ChestParticles;
    ParticleEffectsPool HeadParticles;
    List<ParticleEffectsPool> OnHitEffects;
    List<ParticleEffectsPool> OnDeathEffects;

    public void SetArmourForHP(int HP)
    {
        GetPools();
        switch (HP)
        {
            case 1:
                armourPieces.ForEach(n => n.SetActive(false));
                break;
            case 2:
                armourPieces[3].SetActive(false);
                armourPieces[2].SetActive(false);
                armourPieces[1].SetActive(false);
                armourPieces[0].SetActive(true);
                break;
            case 3:
                armourPieces[3].SetActive(false);
                armourPieces[2].SetActive(false);
                armourPieces[1].SetActive(true);
                armourPieces[0].SetActive(true);
                break;
            case 4:
                armourPieces.ForEach((n) => n.SetActive(true));
                break;
        }
    }
    public void GenerateHitParticles(int HP)
    {
        GetPools();
        Vector3 enemyPosition = self.transform.position;  // Use the enemy's position as the base
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1f, 3.5f), Random.Range(-0.5f, 0.5f));
        Vector3 particlePosition = enemyPosition + randomOffset;

        //apply hit effects based on the remaining HP
        switch (HP)
        {
            case 0:
                break;
            case 1:
                armourPieces[0].SetActive(false);
                ChestParticles.GetGameObjectTransform(enemyPosition, Quaternion.identity);
                break;
            case 2:
                HeadParticles.GetGameObjectTransform(armourPieces[1].transform.position, Quaternion.identity);

                armourPieces[1].SetActive(false);
                break;
            case 3:
                SideParticles.GetGameObjectTransform(armourPieces[2].transform.position, self.transform.rotation);
                SideParticles.GetGameObjectTransform(armourPieces[3].transform.position, self.transform.rotation);

                armourPieces[2].SetActive(false);
                armourPieces[3].SetActive(false);
                break;
        }

        float shakeMult = shakeCamMultDead;

        if (HP > 0)
        {
            shakeMult = shakeCamMultHit;
        }
        else
        {
            OnDeathEffects.ForEach(effectPool =>  effectPool.GetGameObjectTransform(particlePosition, Quaternion.identity));
        }

        camShaker.ShakeCamera(Vector3.one * shakeMult);

        // Apply OnHit effects only if the damage is from the player
        if (self.damageSource == "player")
        {
            OnHitEffects.ForEach(effectPool => effectPool.GetGameObjectTransform(particlePosition, Quaternion.identity));
        }
    }
    void GetPools()
    {
        if (smokePool == null)
        {
            smokePool = ObjectPoolManager.instance.VFXPoolsManager.GetSmokePoolForKey(smokePoolKey);
        }
        if (particleEffectsPoolManager == null)
        {
            particleEffectsPoolManager = ObjectPoolManager.instance.particleEffectsPoolManager;
        }
        if (OnHitEffects == null)
        {
            OnHitEffects = particleEffectsPoolManager.OnHitEffectsPool;
        }
        if(OnDeathEffects == null)
        {
            OnDeathEffects = particleEffectsPoolManager.OnDeathEffectsPool;
        }
        if (SideParticles == null)
        {
            SideParticles = particleEffectsPoolManager.SideArmourPiecesPool;
        }
        if (ChestParticles == null)
        {
            ChestParticles = particleEffectsPoolManager.ChestArmourPiecesPool;
        }
        if (HeadParticles == null)
        {
            HeadParticles = particleEffectsPoolManager.HeadArmourPiecesPool;
        }
    }
    public void PlayOnDestructionSmoke()
    {
        smokePool.GetGameObjectTransform(self.transform.position, Quaternion.identity);
    }
}
