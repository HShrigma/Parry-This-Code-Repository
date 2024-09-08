using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioClip introMusic;
    [SerializeField] AudioClip loopMusic;
    [SerializeField] AudioSource musicSRC;
    void Start()
    {
        mixer.SetFloat(
            "MasterVol", 
            Mathf.Log10(PlayerPrefsManager.GetMasterVol()) * 20
            );

        musicSRC.PlayOneShot(introMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicSRC.isPlaying) 
        { 
            musicSRC.PlayOneShot(loopMusic);
        }
    }
}
