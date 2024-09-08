using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class MeleeEnemyWeaponVFXManager : MonoBehaviour
{
    [SerializeField] Enemy self;
    List<GameObject> weapons;
    
    [SerializeField] MeleeEnemyParryVFXManager parryVFXManager;
    
    [Header("Weapon VFX")]
    [SerializeField] VisualEffect oneWeaponVFX;
    [SerializeField] VisualEffect bothWeaponsVFX;
    VisualEffect activeVFX;
    
    [Header("Weapon Particles")]
    [SerializeField] List<ParticleSystem> leftWeaponParticles;
    [SerializeField] List<ParticleSystem> rightWeaponParticles;
    List<ParticleSystem> activeParticles = new List<ParticleSystem>();

    public UnityEvent OnWeaponsReceived;
    
    void Start()
    {
        OnWeaponsReceived.AddListener(delegate { parryVFXManager.ReceiveWeapons(weapons); });
        StartCoroutine(GetWeaponsAfterTime());
    }
    IEnumerator GetWeaponsAfterTime() 
    {
        yield return new WaitForEndOfFrame();
        weapons = self.GetWeaponNames();
        if (weapons.Count == 2)
        {
            activeVFX = bothWeaponsVFX;
            activeParticles = leftWeaponParticles;
            activeParticles.AddRange(rightWeaponParticles);
        }
        else
        {
            activeVFX = oneWeaponVFX;

            if (weapons[0].name == "L_Knife")
            {
                activeParticles = leftWeaponParticles;
            }
            else
            {
                activeParticles = rightWeaponParticles;
            }
        }
        OnWeaponsReceived.Invoke();
    }

    public void EnableWeaponVFX()
    {
        activeParticles.ForEach(p => p.Play());
        if(activeVFX != null)
        {
            activeVFX.SendEvent("PlayCustom");
        }
    }

    public void DisableWeaponVFX()
    {
        activeParticles.ForEach(p => p.Stop());
    }
}