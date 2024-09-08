using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void SetStats()
    {
        base.SetStats();
        self_state = Enemy.enemyStates.Idle;
    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void ExecuteUpdate()
    {
        helper.LookAtPlayerOnYAxis();
    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
    }
}
