using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    protected override void Awake()
    {
        self_state = PlayerController.playerFSMMain.Idle;
    }
    public override void ExecuteUpdate()
    {
    }

    public override void ExecuteFixedUpdate()
    {
    }

    public override void OnStateChanged()
    {
        base.OnStateChanged();
        helper.setRBDrag("idle");
    }
}
