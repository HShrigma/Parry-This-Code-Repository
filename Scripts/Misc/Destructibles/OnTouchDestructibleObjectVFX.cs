using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OnTouchDestructibleObjectVFX : DestructibleObjectTouch
{
    [SerializeField] VisualEffect vfx;

    bool destroyed = false;

    public override void TriggerDestruction()
    {
        if (!destroyed)
        {
            Debug.Log("Destruction Triggered");
            DisableDestructibleComponents();
            SetPitchAndPlayAudio();
            vfx.SendEvent("CustomPlay");
            destroyed = true;
        }
    }
}
