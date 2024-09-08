using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingState : PlayerState
{
    enum dashState
    {
        canDash,
        dashStart,
        dashing,
        dashEnd,
        dashRecovery
    }

    [Header("Dash Parameters")]
    [SerializeField] dashState subState;
    [SerializeField] float dashSpeedMult = 50f;
    [SerializeField] float dashCooldownSeconds;
    [SerializeField] PlayerDashVFX dashVFX;
    protected override void Awake()
    {
        self_state = PlayerController.playerFSMMain.Dashing;
        subState = dashState.canDash;
    }
    public override void ExecuteFixedUpdate()
    {
        Dash();
    }

    public override void ExecuteUpdate()
    {
        switch (subState)
        {
            case dashState.canDash:
                break;
            case dashState.dashStart:
                helper.RotateOnInput();
                break;
            case dashState.dashing:
                break;
            case dashState.dashEnd:
                helper.setRBDrag("idle");
                break;
            case dashState.dashRecovery:
                break;
        }
    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (PlayerController.instance.state == PlayerController.playerFSMMain.Hurt)
        {
            subState = dashState.canDash;
        }
    }
    private void Update()
    {
        //check for cooldown
        if (subState == dashState.canDash)
        {
            //ensure not interrupting states with higher priority
            //Dashing > idle, movement, attack
            //Dashing < hurt, dead
            if (PlayerController.playerFSMMain.Dashing > PlayerController.instance.state && 
                Input.GetButtonDown("Dash"))
            {
                PlayerController.instance.SetState(self_state);
                helper.setRBDrag(0f, 0f);
                subState = dashState.dashStart;
                helper.SFXManager.PlayDash();
                dashVFX.PlayDashStartVFX();

            }
        }
    }

    void Dash()
    {
        if (subState == dashState.dashStart || subState == dashState.dashing)
        {
            helper.EnableInvulnerable();
            helper.MoveWithForceMode(ForceMode.Acceleration, dashSpeedMult);
        }
    }
    #region Animation Event Methods
    public void SetDashing()
    {
        subState = dashState.dashing;
    }
    public void SetDashEnd()
    {
        dashVFX.PlayDashEndVFX();
        helper.SFXManager.PlayLand();
        subState = dashState.dashEnd;
    }
    IEnumerator WaitThenDash()
    {
        yield return new WaitForSeconds(dashCooldownSeconds);
        subState = dashState.canDash;
    }
    public void DashRecovery()
    {
        subState = dashState.dashRecovery;
        helper.DisableInvulnerable();
        PlayerController.instance.ResetState();
        StartCoroutine(WaitThenDash());
    }
    #endregion
}
