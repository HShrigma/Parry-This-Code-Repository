using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    int buttonIndex = 0;
    [SerializeField] List<AudioClip> clickSounds;
    [SerializeField] List<AudioClip> hoverSounds;
    [SerializeField] AudioSource buttonSource;

    void Start()
    {
        SetMasterVolLog(PlayerPrefsManager.GetMasterVol());
        SetMusicVolLog(PlayerPrefsManager.GetMusicVol());
        SetSFXVolLog(PlayerPrefsManager.GetSFXVol());
    }

    void Update()
    {

    }

    public void SetMasterVolLog(float value)
    {
        PlayerPrefsManager.SetMasterVol(value);
        value = Mathf.Log10(value) * 20;
        mixer.SetFloat("MasterVol", value);
    }
    public void SetMusicVolLog(float value)
    {
        PlayerPrefsManager.SetMusicVol(value);
        value = Mathf.Log10(value) * 20;
        mixer.SetFloat("MusicVol", value);
    }
    public void SetSFXVolLog(float value)
    {
        PlayerPrefsManager.SetSFXVol(value);
        value = Mathf.Log10(value) * 20;
        mixer.SetFloat("SFXVol", value);
    }

    public void PlayHover()
    {
        if (!buttonSource.isPlaying)
        {
            buttonSource.PlayOneShot(hoverSounds[buttonIndex]);
            IncrementButtonIndex();
        }
    }

    public void PlayHoverForce()
    {
        buttonSource.PlayOneShot(hoverSounds[buttonIndex]);
        IncrementButtonIndex();
    }

    public void PlayClick()
    {
            buttonSource.PlayOneShot(clickSounds[buttonIndex]);
            IncrementButtonIndex();
    }

    void IncrementButtonIndex()
    {
        int limit = GetButtonIndexLimit();
        buttonIndex++;
        if(buttonIndex >= limit)
        {
            buttonIndex = 0;
        }
    }

    int GetButtonIndexLimit()
    {
        if (hoverSounds.Count <= clickSounds.Count)
        {
            return hoverSounds.Count;
        }
        return clickSounds.Count;
    }
}
