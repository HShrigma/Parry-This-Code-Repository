using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParriedState : EnemyState
{
    //Serialized field to avoid issues with non-melee enemies
    [SerializeField] MeleeEnemyParryVFXManager parryVFXManager;
    public override void SetStats()
    {
        base.SetStats();
        self_state = Enemy.enemyStates.Parried;
    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void ExecuteUpdate()
    {

    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (self.state == self_state)
        {
            if (parryVFXManager != null)
            {
                parryVFXManager.PlayParryVFX();
            }
            gm.IncrementScore("parry");
            helper.SFXManager.PlayParried();
        }
    }
}
