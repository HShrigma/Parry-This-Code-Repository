using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState : EnemyState
{
    [SerializeField] EnemyNavMeshAgent agent;
    public override void SetStats()
    {
        base.SetStats();
        self_state = Enemy.enemyStates.Moving;
    }
    public override void ExecuteFixedUpdate()
    {
        agent.MoveTowardsPlayer();
       // helper.MoveForwardWithMod(speed);
    }

    public override void ExecuteUpdate()
    {
    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (self.state != self_state)
        {
            agent.SetEnabled(false);
        }
        else
        {
            agent.SetEnabled(true);
        }
    }
    public void OnNavMeshMoveCheck(bool value)
    {
        self.SetState(Enemy.enemyStates.Idle);
    }
}
