using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerState
{
    [SerializeField] List<GameObject> hitboxes;
    [SerializeField] int attackCounter = 0;
    [SerializeField] bool inputBuffered = false;
    [SerializeField] float attackMovementSpeed = 1f;
    [SerializeField] PlayerSlashVFXProcessor vfxProcessor;
    bool attackMoveEnabled = false;
    bool hasPlayedFX = false;
    bool canBuffer = false;

    public enum attackStates
    {
        canAttack,
        attackStart,
        attackActive,
        attackCancel,
        attackRecovery
    }
    [SerializeField] attackStates subState = attackStates.canAttack;
    protected override void Awake()
    {
        self_state = PlayerController.playerFSMMain.Attacking;
        subState = attackStates.canAttack;
    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (!isEnabled)
        {
            ResetParams();
        }
    }
    public override void ExecuteFixedUpdate()
    {
        if (attackMoveEnabled && helper.MoveInput.magnitude != 0)
        {
            float multiplier = attackMovementSpeed;
            switch (attackCounter)
            {
                case 0:
                    multiplier *= .7f;
                    break;
                case 1:
                    multiplier *= .55f;
                    break;
                case 2:
                    multiplier *= .75f;
                    break;
            }
            helper.MoveWithForceMode(ForceMode.VelocityChange, multiplier);
        }
    }

    public override void ExecuteUpdate()
    {
        PlayerController.instance.SetAnimatorAttackCounter(attackCounter);
        switch (subState)
        {
            case attackStates.canAttack:
                break;
            case attackStates.attackStart:
                helper.RotateOnInput();
                break;
            case attackStates.attackActive:
                canBuffer = true;
                if (Input.GetButtonDown("Attack"))
                {
                    inputBuffered = true;
                }
                hitboxes[attackCounter].SetActive(true);

                if (!hasPlayedFX)
                {
                    vfxProcessor.PlaySlash(attackCounter);
                    helper.InitCameraShake(.15f);
                    helper.SFXManager.PlaySlash(attackCounter);
                    hasPlayedFX = true;
                }

                break;
            case attackStates.attackCancel:
                if (helper.Invulnerable)
                {
                    helper.DisableInvulnerable();
                }
                hitboxes[attackCounter].SetActive(false);
                hasPlayedFX = false;
                if (canBuffer)
                {
                    if (Input.GetButtonDown("Attack"))
                    {
                        inputBuffered = true;
                        canBuffer = false;
                    }
                }
                if (inputBuffered)
                {
                    IncrementAttackCounter();
                    if (attackCounter < 3)
                    {
                        subState = attackStates.canAttack;
                    }
                    if (attackCounter >= 3)
                    {
                        attackCounter = 0;
                    }
                }
                inputBuffered = false;
                break;
            case attackStates.attackRecovery:
                attackCounter = 0;
                PlayerController.instance.ResetState();
                break;
        }
    }
    void Update()
    {
        switch (subState)
        {
            case attackStates.canAttack:
                if (Input.GetButtonDown("Attack"))
                {
                    PlayerController.instance.SetState(self_state);
                }
                break;
        }
    }
    void IncrementAttackCounter()
    {
        attackCounter++;
    }
    void ResetParams()
    {
        attackMoveEnabled = false;
        hasPlayedFX = false;
        inputBuffered = false;
        attackCounter = 0;
        hitboxes.ForEach(hb => hb.SetActive(false));
        subState = attackStates.canAttack;
    }
    #region Animation Event Methods
    public void EnableAttackMove()
    {
        attackMoveEnabled = true;
    }
    public void DisableAttackMove()
    {
        attackMoveEnabled = false;
    }
    public void ToggleAttackSubState(attackStates _subState)
    {
        subState = _subState;
    }
    #endregion
}

