using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestructibleObjectTrigger : DestructibleObject
{
    [Header("VFX")]
    [SerializeField] VisualEffect vFX;
    bool destroyed = false;
    public override void TriggerDestruction()
    {
        if (!destroyed) 
        {
            destroyed = true;
            DoOnDestructionBase();
        }
    }

    protected virtual void DoOnDestructionBase()
    {
        SetPitchAndPlayAudio();
        DisableDestructibleComponents();
        vFX.SendEvent("CustomPlay");
    }
}
