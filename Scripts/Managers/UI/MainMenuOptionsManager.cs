using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOptionsManager : MonoBehaviour
{
    [SerializeField] Slider mainSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;

    [SerializeField] SliderColorTint mainTint;

    void Start()
    {
        //Set Initial Slider value based on saved values
        mainSlider.value = PlayerPrefsManager.GetMasterVol();
        musicSlider.value = PlayerPrefsManager.GetMusicVol();
        SFXSlider.value = PlayerPrefsManager.GetSFXVol();
    }
    void OnEnable()
    {
        mainSlider.Select();
        mainTint.SetSelectTint();
    }

    private void Update()
    {
    }
}
