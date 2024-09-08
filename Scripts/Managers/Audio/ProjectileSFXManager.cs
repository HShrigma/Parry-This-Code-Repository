using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSFXManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] AudioSource bulletHitSource;
    [SerializeField] AudioSource bulletRicochetSource;
    [SerializeField] AudioSource bulletParrySource;
    [SerializeField] AudioSource grenadeParrySource;
    [SerializeField] AudioSource grenadeBounceSource;
    [SerializeField] AudioSource grenadeExplodeSource;
    [SerializeField] AudioSource bazookaBeepSource;
    [SerializeField] AudioSource bazookaLastBeepSource;
    [SerializeField] AudioSource bazookaExplodeSource;

    [Header("Clips")]
    [SerializeField] List<AudioClip> bulletHitClips;
    [SerializeField] List<AudioClip> bulletRicochetClips;
    [SerializeField] List<AudioClip> bulletParryClips;
    [SerializeField] List<AudioClip> grenadeBounceClips;
    [SerializeField] List<AudioClip> grenadeExplodeClips;
    [SerializeField] List<AudioClip> bazookaBeepClips;
    [SerializeField] AudioClip bazookaLastBeepClip;
    [SerializeField] List<AudioClip> bazookaExplodeClips;
    void PlaySoundRandom(List<AudioClip> clips, AudioSource src)
    {
        int index = Random.Range(0, clips.Count);
        src.PlayOneShot(clips[index]);
    }
    public void PlayBulletHit()
    {
        PlaySoundRandom(bulletHitClips, bulletHitSource);
    }

    public void PlayBulletRicochet()
    {
        PlaySoundRandom(bulletRicochetClips, bulletRicochetSource);
    }
    public void PlayBulletParry()
    {
        PlaySoundRandom(bulletParryClips, bulletParrySource);
    }

    public void PlayGrenadeParry()
    {
        PlaySoundRandom(bulletParryClips, grenadeParrySource);
    }
    public void PlayGrenadeBounce()
    {
        PlaySoundRandom(grenadeBounceClips, grenadeBounceSource);
    }
    public void PlayGrenadeExplode()
    {
        PlaySoundRandom(grenadeExplodeClips, grenadeExplodeSource);
    }
    public void PlayBazookaBeep(int index)
    {
        bazookaBeepSource.PlayOneShot(bazookaBeepClips[index]);
    }
    public void PlayBazookaLastBeep()
    {
        bazookaLastBeepSource.PlayOneShot(bazookaLastBeepClip);
    }
    public void PlayBazookaExplode()
    {
        PlaySoundRandom(bazookaExplodeClips, bazookaExplodeSource);
    }
    public int GetBeepsCount()
    {
        return bazookaBeepClips.Count;
    }
}
