using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] EnemyAttackingMeleeState attackingMeleeState;
    protected override void CheckRange()
    {
        if (InMeleeAttackRange())
        {
            HandleRangeCheckMelee();
        }
        else
        {
            SetStateBasedOnNavMesh();
        }
    }
    public override void SetAnimatorParameters(int rWeaponIndex, int lWeaponIndex)
    {
        base.SetAnimatorParameters(rWeaponIndex, lWeaponIndex);
        animator.SetBool("Melee", true);
    }
    bool CanMeleeAttack()
    {
        return attackingMeleeState.attackState ==
                    EnemyAttackingMeleeState.meleeAttackSubstate.canAttack;
    }

    void HandleRangeCheckMelee()
    {
        if (ColliderIsOfLayer(meleeRaycast.collider, "Terrain"))
        {
            SetStateBasedOnNavMesh();
        }
        else
        {
            //see if can attack
            if (CanMeleeAttack())
            {
                SetState(enemyStates.AttackingMelee);
            }
            else
            {
                SetState(enemyStates.Idle);
            }
        }
    }
    bool InMeleeAttackRange()
    {
        Vector3 lookPosition = Helper.GetDirToPlayer(transform);
        //Debug.DrawRay(transform.position, lookPosition * rangedDist, Color.magenta);
        return Physics.Raycast(transform.position, lookPosition, out meleeRaycast, meleeDist, playerTerrainLayerMask);
    }
}
