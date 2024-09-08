using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFXManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] AudioSource deadYellSource;
    [SerializeField] AudioSource deadImpactSource;
    [SerializeField] AudioSource hurtImpactSource;
    [SerializeField] AudioSource hurtArmourBreakSource;
    [SerializeField] AudioSource hurtYellSource;
    [SerializeField] AudioSource smokeEntrySource;
    [SerializeField] AudioSource smokeExitSource;
    [SerializeField] AudioSource parrySource;
    [SerializeField] AudioSource meleeSlashSource;
    [SerializeField] AudioSource meleeStabSource;
    [SerializeField] AudioSource meleeAirStabSource;
    [SerializeField] AudioSource gunShootSource;
    [SerializeField] AudioSource gunCockingSource;
    [SerializeField] AudioSource uziShootSource;
    [SerializeField] AudioSource uziCockingSource;
    [SerializeField] AudioSource grenadeShootSource;
    [SerializeField] AudioSource grenadeCockingSource;
    [SerializeField] AudioSource bazookaShootSource;
    [SerializeField] AudioSource bazookaCockingSource;


    [Header("Clips")]
    [SerializeField] List<AudioClip> deadYellClips;
    [SerializeField] List<AudioClip> deadImpactClips;
    [SerializeField] List<AudioClip> hurtImpactClips;
    [SerializeField] List<AudioClip> hurtArmourBreakClips;
    [SerializeField] List<AudioClip> hurtYellClips;
    [SerializeField] List<AudioClip> smokeEntryClips;
    [SerializeField] List<AudioClip> smokeExitClips;
    [SerializeField] List<AudioClip> parryClips;
    [SerializeField] List<AudioClip> meleeSlashClips;
    [SerializeField] List<AudioClip> meleeStabClips;
    [SerializeField] List<AudioClip> meleeAirStabClips;
    [SerializeField] List<AudioClip> gunShootClips;
    [SerializeField] List<AudioClip> gunCockingClips;
    [SerializeField] List<AudioClip> grenadeShootClips;
    [SerializeField] List<AudioClip> bazookaShootClips;
    void PlaySoundRandom(List<AudioClip> clips, AudioSource src)
    {
        int index = Random.Range(0, clips.Count);
        src.PlayOneShot(clips[index]);
    }
    public void PlaySmokeEntry()
    {
        PlaySoundRandom(smokeEntryClips, smokeEntrySource);
    }

    public void PlaySmokeExit()
    {
        PlaySoundRandom(smokeEntryClips, smokeExitSource);
    }

    public void PlayHurtImpact()
    {
        PlaySoundRandom(hurtImpactClips, hurtImpactSource);
    }

    public void PlayDeadImpact()
    {
        PlaySoundRandom(deadImpactClips, deadImpactSource);
    }

    public void PlayHurtYell()
    {
        PlaySoundRandom(hurtYellClips, hurtYellSource);
    }

    public void PlayArmourBreak()
    {
        PlaySoundRandom(hurtArmourBreakClips, hurtArmourBreakSource);
    }

    public void PlayDeadYell()
    {
        PlaySoundRandom(deadYellClips, deadYellSource);
    }

    public void PlayParried()
    {
        PlaySoundRandom(parryClips, parrySource);
    }

    void PlaySlash()
    {
        PlaySoundRandom(meleeSlashClips, meleeSlashSource);
    }
    void PlayStab()
    {
        PlaySoundRandom(meleeStabClips, meleeStabSource);
    }

    void PlayAirStab()
    {
        PlaySoundRandom(meleeAirStabClips, meleeAirStabSource);
    }

    public void PlayMeleeAttack(int weaponCount, int attackCount)
    {
        if (weaponCount == 1)
        {
            PlaySlash();
        }
        else
        {
            if (attackCount == 1)
            {
                PlayAirStab();
            }
            else
            {
                PlayStab();
            }
        }
    }

    void PlayGunCocking()
    {
        PlaySoundRandom(gunCockingClips, gunCockingSource);
    }
    void PlayGunShooting()
    {
        PlaySoundRandom(gunShootClips, gunShootSource);
    }

    void PlayUziShooting()
    {
        PlaySoundRandom(gunShootClips, uziShootSource);
    }
    void PlayUziCocking()
    {
        PlaySoundRandom(gunCockingClips, uziCockingSource);
    }

    void PlayGrenadeCocking()
    {
        PlaySoundRandom(gunCockingClips, grenadeCockingSource);
    }

    void PlayGrenadeShooting()
    {
        PlaySoundRandom(grenadeShootClips, grenadeShootSource);
    }

    void PlayBazookaCocking()
    {
        PlaySoundRandom(gunCockingClips, bazookaCockingSource);
    }

    void PlayBazookaShooting()
    {
        PlaySoundRandom(bazookaShootClips, bazookaShootSource);
    }
    public void PlayCockingForWeaponIndex(int index)
    {
        switch (index)
        {
            case 2:
                PlayGunCocking();
                break;
            case 3:
                PlayUziCocking();
                break;
            case 4:
                PlayGrenadeCocking();
                break;
            case 5:
                PlayBazookaCocking();
                break;
            default:
                PlayGunCocking();
                Debug.LogError($"No such ranged weapon index: {index}");
                break;
        }
    }
    public void PlayShootingForWeaponIndex(int index)
    {
        switch (index)
        {
            case 2:
                PlayGunShooting();
                break;
            case 3:
                PlayUziShooting();
                break;
            case 4:
                PlayGrenadeShooting();
                break;
            case 5:
                PlayBazookaShooting();
                break;
            default:
                PlayGunShooting();
                Debug.LogError($"No such ranged weapon index: {index}");
                break;
        }
    }
}