using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    int attackIndex;
    [SerializeField] EnemyAttackingRangedState attackingRangedState;
    [SerializeField] int rangedAttackVariationsCount = 2;
    protected override void CheckRange()
    {
        Vector3 lookPosition = Helper.GetDirToPlayer(transform);
        if (InRangedAttackRange())
        {
            HandleRangeCheckRanged();
        }
        else
        {
            SetStateBasedOnNavMesh();
        }
    }
    public override void SetStats()
    {
        base.SetStats();
        attackIndex = Random.Range(0, rangedAttackVariationsCount);
    }
    public override void SetAnimatorParameters(int rWeaponIndex, int lWeaponIndex)
    {
        base.SetAnimatorParameters(rWeaponIndex, lWeaponIndex);
        animator.SetBool("Melee", false);
    }
    void CycleAttackIndex()
    {
        attackIndex++;
        attackIndex %= rangedAttackVariationsCount;
    }

    bool CanRangedAttack()
    {
        return attackingRangedState.AttackState ==
                           EnemyAttackingRangedState.rangedAttackSubstate.canAttack;
    }
    void HandleRangeCheckRanged()
    {
        if (ColliderIsOfLayer(rangedRaycast.collider, "Terrain"))
        {
            SetStateBasedOnNavMesh();
        }
        else
        {
            //see if can attack
            if (CanRangedAttack())
            {
                CycleAttackIndex();
                animator.SetInteger("Gun_Variation", attackIndex);
                SetState(enemyStates.AttackingRanged);
            }
            else
            {
                SetState(enemyStates.Idle);
            }
        }
    }
    bool InRangedAttackRange()
    {
        Vector3 lookPosition = Helper.GetDirToPlayer(transform);
        //Debug.DrawRay(transform.position, lookPosition * rangedDist, Color.magenta);
        return Physics.Raycast(transform.position, lookPosition, out rangedRaycast, rangedDist, playerTerrainLayerMask);
    }
}
