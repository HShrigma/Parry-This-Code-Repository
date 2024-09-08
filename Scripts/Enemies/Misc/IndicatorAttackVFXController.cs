using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class IndicatorAttackVFXController : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    public void PlayAttackIndicator()
    {
        if (effect != null)
        {
            effect.Play();
        }
    }

    public void StopAttackIndicator()
    {
        if (effect != null)
        {
            effect.Stop();
        }
    }

}
