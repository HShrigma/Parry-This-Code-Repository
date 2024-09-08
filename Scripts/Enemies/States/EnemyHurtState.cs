using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : EnemyState
{
    [SerializeField] float hurtMovespeed = 5f;
    bool moveHurt;
    bool playedEffects;
    int hurtNum = 0;
    public override void SetStats()
    {
        base.SetStats();
        self_state = Enemy.enemyStates.Hurt;
        playedEffects = false;
    }
    public override void ExecuteFixedUpdate()
    {
        if (moveHurt)
        {
            helper.LookAtPlayerOnYAxis();
            helper.MoveForwardWithMod(hurtMovespeed * -1f);
        }
    }

    public override void ExecuteUpdate()
    {
    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (self.state != self_state)
        {
            DisableMoveHurt();
            playedEffects = false;
        }
        else
        {
            if (!playedEffects)
            {
                gm.IncrementScore("hurt");
                helper.SFXManager.PlayHurtYell();
                helper.SFXManager.PlayArmourBreak();
                playedEffects = true;
            }
        }
    }
    public void EnableMoveHurt()
    {
        moveHurt = true;
    }
    public void DisableMoveHurt()
    {
        moveHurt = false;
    }
    public void ToggleJustHurtTrue()
    {
        self.animator.SetBool("Just_Hurt", true);
    }
    public void ToggleJustHurtFalse()
    {
        self.animator.SetBool("Just_Hurt", false);
    }
    public void GetHurt()
    {
        self.SetState(self_state);
        ToggleJustHurtTrue();
        int toSet = Random.Range(0, 3);
        if (toSet == hurtNum)
        {
            toSet = (toSet + 1 ) % 3;
        }
        self.animator.SetInteger("Hurt_Variation", toSet);
        hurtNum = self.animator.GetInteger("Hurt_Variation");
    }
}
