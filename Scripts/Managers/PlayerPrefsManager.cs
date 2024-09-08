using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static int GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            return PlayerPrefs.GetInt("HighScore");
        }
        return 0;
    }
    public static int GetMaxWave()
    {
        if (PlayerPrefs.HasKey("MaxWave"))
        {
            return PlayerPrefs.GetInt("MaxWave");
        }
        return 0;
    }
    public static float GetMasterVol()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            return PlayerPrefs.GetFloat("MasterVol");
        }
        return 1f;
    }
    public static float GetMusicVol()
    {
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            return PlayerPrefs.GetFloat("MusicVol");
        }
        return 1f;
    }
    public static float GetSFXVol()
    {
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            return PlayerPrefs.GetFloat("SFXVol");
        }
        return 1f;
    }
    public static void SetMasterVol(float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("MasterVol", value);
    }
    public static void SetMusicVol(float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("MusicVol", value);
    }
    public static void SetSFXVol(float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("SFXVol", value);
    }

    public static void SetMaxWave(int value)
    {
        value = Mathf.Clamp(value,0,int.MaxValue);
        PlayerPrefs.SetInt("MaxWave", value);
    }
    public static void SetHighScore(int value)
    {
        value = Mathf.Clamp(value, 0, int.MaxValue);
        PlayerPrefs.SetInt("HighScore", value);
    }
}
