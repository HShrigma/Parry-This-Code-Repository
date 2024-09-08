using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackingRangedState : EnemyState
{
    [Header("Gameplay Parameters")]
    [SerializeField] float cooldownTime = 2f;
    [SerializeField] float initCooldownCoefficient;
    [SerializeField] Vector3 lookOffset;
    [Header("Projectile Parameters")]
    [SerializeField] Transform L_Weapon_Tip;
    [SerializeField] Transform R_Weapon_Tip;
    [SerializeField] string projectilePoolKey;
    ObjectPoolBase projectilePool;
    [Header("Effects")]
    [SerializeField] IndicatorAttackVFXController indicatorController;
    [SerializeField] RangedEnemyWeaponVFXManager rangedVFX;
    [SerializeField] UIIndicatorAttackController indicatorUI;

    List<Transform> activeTips;
    Vector3 lookDirection;
    bool firstAttack = true;
    public enum rangedAttackSubstate
    {
        canAttack,
        AttackRotate,
        Firing,
        endAttack,
        attackCooldown
    }

    public rangedAttackSubstate AttackState; //{ get; private set; }
    public override void SetStats()
    {
        base.SetStats();
        self_state = Enemy.enemyStates.AttackingRanged;
        AttackState = rangedAttackSubstate.canAttack;
        projectilePool = ObjectPoolManager.instance.ProjectilePoolsManager.GetPoolForEnemyType(projectilePoolKey);
    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void ExecuteUpdate()
    {
        switch (AttackState)
        {
            case rangedAttackSubstate.AttackRotate:
                helper.LookAtPlayerOnYAxis(lookOffset);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (self_state != self.state)
        {
            if (AttackState != rangedAttackSubstate.canAttack && AttackState != rangedAttackSubstate.attackCooldown)
            {
                AttackState = rangedAttackSubstate.canAttack;
            }
            if (self.state != Enemy.enemyStates.AttackingMelee)
            {
                indicatorController.StopAttackIndicator();
            }
        }
    }
    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if (self_state == self.state && firstAttack)
        {
            SetAttackCooldown(initCooldownCoefficient);
            firstAttack = false;
        }
    }

    IEnumerator WaitForAttackCooldown(float mod = 1f)
    {
        AttackState = rangedAttackSubstate.attackCooldown;
        yield return new WaitForSeconds((cooldownTime * mod) + Random.Range(-0.1f, 0.1f));
        AttackState = rangedAttackSubstate.canAttack;
    }
    public void SetAttackCooldown(float mod = 1f)
    {
        self.SetState(Enemy.enemyStates.Idle);
        StartCoroutine(WaitForAttackCooldown(mod));
    }

    public void SetAttackRotate()
    {
        AttackState = rangedAttackSubstate.AttackRotate;
        indicatorController.PlayAttackIndicator();
        indicatorUI.PlayOrStopParticles();
        if (activeTips == null)
        {
            InitActiveTips();
        }
        for (int i = 0; i < activeTips.Count; i++)
        {
            int weaponIndex = GetWeaponIndex(i);
            helper.SFXManager.PlayCockingForWeaponIndex(weaponIndex);
        }
    }

    public void SetFiring()
    {
        AttackState = rangedAttackSubstate.Firing;
        indicatorController.StopAttackIndicator();
    }

    public void SetEndAttack()
    {
        AttackState = rangedAttackSubstate.endAttack;
        indicatorUI.PlayOrStopParticles(false);
    }

    public void LaunchProjectile()
    {
        if (activeTips == null)
        {
            InitActiveTips();
        }
        if (self.state == self_state)
        {
            for (int i = 0; i < activeTips.Count; i++)
            {
                int weaponIndex = GetWeaponIndex(i);
                //Instantiate(projectile, activeTips[i].position, self.transform.rotation);
                projectilePool.GetGameObjectTransform(activeTips[i].position, self.transform.rotation);
                helper.SFXManager.PlayShootingForWeaponIndex(weaponIndex);
                rangedVFX.PlayRangedVFXForWeaponIndex(weaponIndex,activeTips[i]);
            }
        }
    }
    int GetWeaponIndex(int activeTipsIndex)
    {
        if (activeTips[activeTipsIndex].name.StartsWith("L"))
        {
            return self.animator.GetInteger("L_Weapon");
        }
        else
        {
            return self.animator.GetInteger("R_Weapon");
        }
    }
    void InitActiveTips()
    {
        activeTips = new List<Transform>();
        if (self.animator.GetInteger("L_Weapon") > 1)
        {
            activeTips.Add(L_Weapon_Tip);
        }
        if (self.animator.GetInteger("R_Weapon") > 1)
        {
            activeTips.Add(R_Weapon_Tip);
        }
    }
}
