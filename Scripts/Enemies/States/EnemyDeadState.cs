using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public override void SetStats()
    {
        base.SetStats();
        self_state = Enemy.enemyStates.Dead;
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
    }
    public void Die()
    {
        helper.SFXManager.PlayDeadYell();
        self.SetState(self_state);
        self.animator.SetInteger("Hurt_Variation", Random.Range(0, 3));
        helper.DisableCollisions();
    }
    public void DestroyOnDeath()
    {
        self.ArmourManager.PlayOnDestructionSmoke();
        gm.IncrementScore("dead");
        helper.SFXManager.PlaySmokeExit();
        self.ReturnToPool();
    }

}
