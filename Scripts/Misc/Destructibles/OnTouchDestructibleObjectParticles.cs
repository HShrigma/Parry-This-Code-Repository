using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnTouchDestructibleObjectParticles : DestructibleObjectTouch
{
    [SerializeField] ParticleSystem particles;

    bool destroyed = false;

    public override void TriggerDestruction()
    {
        if (!destroyed)
        {
            DisableDestructibleComponents();
            SetPitchAndPlayAudio();
            particles.Play();
            destroyed = true;
        }
    }
}
