using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingMeleeState : EnemyState
{
    public enum meleeAttackSubstate
    {
        attackOnCooldown,
        canAttack
    }
    public meleeAttackSubstate attackState { get; private set; }
    bool attackMove = false;
    bool attackRotate = false;
    bool firstAttack = true;
    [Header("Attack Parameters")]
    [SerializeField] float attackMoveSpeed = 10f;
    [SerializeField] float meleeCooldown = 1.5f;
    [SerializeField] float initCooldownCoefficient;
    [SerializeField] float meleeCooldownRandomOffset = .3f;
    [Header("Hitbox")]
    [SerializeField] GameObject hitboxMelee;
    //Serialized field to avoid issues with non-melee enemies
    [SerializeField] MeleeEnemyWeaponVFXManager vFXManager;
    [SerializeField] IndicatorAttackVFXController indicatorController;
    [SerializeField] UIIndicatorAttackController indicatorUI;
    public int AttackCounter { get; private set; }
    public override void SetStats()
    {
        base.SetStats();
        self_state = Enemy.enemyStates.AttackingMelee;
        attackState = meleeAttackSubstate.canAttack;
        AttackCounter = 0;
    }
    public override void ExecuteFixedUpdate()
    {
        if (attackMove)
        {
            helper.MoveForwardWithMod(attackMoveSpeed);
        }
    }

    public override void ExecuteUpdate()
    {
        if (attackRotate)
        {
            helper.LookAtPlayerOnYAxis();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (self.state != self_state)
        {
            DisableAttackMove();
            DisableHitbox();
            DisableAttackRotate();
            AttackCounter=0;
            if(self.state != Enemy.enemyStates.AttackingRanged)
            {
                indicatorController.StopAttackIndicator();
            }
        }
    }

    public void KnifeRecovery(float mod = 1f)
    {
        self.SetState(Enemy.enemyStates.Idle);
        StartCoroutine(setMeleeCooldown(mod));
    }

    IEnumerator setMeleeCooldown(float mod)
    {
        attackState = meleeAttackSubstate.attackOnCooldown;
        yield return new WaitForSeconds(meleeCooldown*mod + Random.Range(-meleeCooldownRandomOffset, meleeCooldownRandomOffset));
        attackState = meleeAttackSubstate.canAttack;
    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (self.state == self_state)
        {
            if (firstAttack)
            {
                KnifeRecovery(initCooldownCoefficient);
                firstAttack = false;
            }
        }

    }
    public void EnableAttackMove()
    {
        attackMove = true;
    }
    public void DisableAttackMove()
    {
        attackMove = false;
    }

    public void EnableHitbox()
    {
        hitboxMelee.SetActive(true);
        AttackCounter++;
        if (vFXManager != null)
        {
            vFXManager.EnableWeaponVFX();
        }
        helper.SFXManager.PlayMeleeAttack(self.GetWeaponNames().Count,AttackCounter);
        indicatorUI.PlayOrStopParticles();
    }
    public void DisableHitbox()
    {
        hitboxMelee.SetActive(false);
        if(vFXManager != null)
        {
            vFXManager.DisableWeaponVFX();
            indicatorUI.PlayOrStopParticles(false);
        }
    }

    public void EnableAttackRotate()
    {
        indicatorController.PlayAttackIndicator();
        indicatorUI.PlayOrStopParticles();
        attackRotate = true;
    }

    public void DisableAttackRotate()
    {
        indicatorController.StopAttackIndicator();
        attackRotate = false;
    }

    public void SetAttackSubstate(meleeAttackSubstate state)
    {
        attackState = state;
    }
}
