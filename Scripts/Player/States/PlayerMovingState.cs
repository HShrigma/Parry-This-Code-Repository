using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerState
{
    protected override void Awake()
    {
        self_state = PlayerController.playerFSMMain.Moving;
    }
    public override void ExecuteFixedUpdate()
    {
        helper.MoveWithForceMode(ForceMode.VelocityChange);
    }

    public override void ExecuteUpdate()
    {
        helper.RotateOnInput();
    }

    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (self_state == PlayerController.instance.state)
        {
            helper.setRBDrag("movement");
        }
        else
        {
            helper.SFXManager.ResetRunIndex();
        }
    }

    public void PlayRunSound() 
    {
        helper.SFXManager.PlayRun();
    }
}
