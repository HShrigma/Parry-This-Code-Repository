using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] List<AudioClip> slashClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> dashClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> landClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> hurtClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> deadClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> runClips = new List<AudioClip>();
    [Header("Audio Sources")]
    [SerializeField] AudioSource slashSource;
    [SerializeField] AudioSource dashSource;
    [SerializeField] AudioSource landSource;
    [SerializeField] AudioSource hurtSource;
    [SerializeField] AudioSource deadSource;
    [SerializeField] AudioSource runSource;

    int runIndex = 0;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySlash(int index)
    {
        index = Mathf.Clamp(index, 0, slashClips.Count - 1);
        slashSource.PlayOneShot(slashClips[index]);
    }

    public void PlayDash()
    {
        int index = Random.Range(0, dashClips.Count);
        dashSource.PlayOneShot(dashClips[index]);
    }

    public void PlayLand()
    {
        int index = Random.Range(0, landClips.Count);
        landSource.PlayOneShot(landClips[index]);
    }

    public void PlayHurt()
    {
        int index = Random.Range(0, hurtClips.Count);
        hurtSource.PlayOneShot(hurtClips[index]);
    }

    public void PlayDead()
    {
        int index = Random.Range(0, deadClips.Count);
        deadSource.PlayOneShot(deadClips[index]);
    }

    public void PlayRun()
    {
        runSource.PlayOneShot(runClips[runIndex]);
        IncrementRunIndex();
    }
    void IncrementRunIndex()
    {
        runIndex++;
        if(runIndex >= runClips.Count)
        {
            ResetRunIndex();
        }
    }
    public void ResetRunIndex()
    {
        runIndex = 0;
    }
}
