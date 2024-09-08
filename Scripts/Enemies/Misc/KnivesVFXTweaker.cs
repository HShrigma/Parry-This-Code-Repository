using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class KnivesVFXTweaker : MonoBehaviour
{
    [SerializeField] EnemyAttackingMeleeState state;
    [SerializeField] VisualEffect effect;

    // Update is called once per frame
    void Update()
    {
        if (state.AttackCounter >= 1)
        {
            effect.SetFloat("Stab Dir",1);
        }
        else
        {
            effect.SetFloat("Stab Dir", -1);
        }
    }
}
