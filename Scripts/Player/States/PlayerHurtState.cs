using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerState
{
    bool hasPlayedFX = false;
    protected override void Awake()
    {
        self_state = PlayerController.playerFSMMain.Hurt;
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
        if (isEnabled)
        {
            helper.SetHurtInvulnerable();
            helper.SetPlayerDamageSource();
            if (!hasPlayedFX)
            {
                helper.InitCameraShake(2.3f);
                helper.InitHitLag();
                helper.SFXManager.PlayHurt();
                hasPlayedFX = true;
            }
        }
        else
        {
            if (PlayerController.instance.state != PlayerController.playerFSMMain.Dead && 
                PlayerController.instance.state != PlayerController.playerFSMMain.Dashing &&
                helper.Invulnerable == false)
            {
                helper.EnableHurtBox();
            }
            hasPlayedFX = false;
        }
    }
}
