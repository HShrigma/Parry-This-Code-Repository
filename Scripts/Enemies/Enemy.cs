using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public abstract class Enemy : MonoBehaviour
{
    public enum enemyStates
    {
        Idle,
        Moving,
        AttackingRanged,
        AttackingMelee,
        Parried,
        Hurt,
        Dead
    }
    public enemyStates state { get; private set; }
    [SerializeField] List<EnemyState> stateManagers;
    [SerializeField] protected List<GameObject> leftWeapons;
    [SerializeField] protected List<GameObject> rightWeapons;
    //[SerializeField] List<GameObject> armourPieces;
    protected LayerMask playerTerrainLayerMask;
    [Header("References")]
    public EnemyStateHelper Helper;
    public EnemyArmourManager ArmourManager;
    [SerializeField] EnemyHPBarController HPBarController;
    [SerializeField] protected EnemyNavMeshAgent navMeshAgent;
    public MeleeEnemyParryVFXManager ParryVFXManager;
    protected PlayerController player;
    public Animator animator;
    int baseHP;
    [Header("Attack Ranges")]
    protected int HP;
    protected RaycastHit meleeRaycast;
    [SerializeField] protected float meleeDist = 5f;
    protected RaycastHit rangedRaycast;
    [SerializeField] protected float rangedDist = 15f;

    [Header("HP Modifiers")]
    [SerializeField] float difficultyCoefficeintMin = 0.5f;
    [SerializeField] float difficultyCoefficeintMax = 0.5f;
    [Header("Events")]
    public UnityEvent OnDeath;
    public UnityEvent OnStateChanged;
    public UnityEvent OnHurt;
    bool onSpawnIdle = true;
    public string damageSource { get; protected set; }
    EnemyPool parentPool;
    public virtual void SetStats()
    {
        Helper.SetStats();
        stateManagers.Where(n => n != null).ToList().ForEach(n => n.SetStats());
        state = enemyStates.Idle;
        StartCoroutine(StopOnSpawnIdle());
        playerTerrainLayerMask = LayerMask.GetMask("Player", "Terrain");
        player = PlayerController.instance;
        SetHPInitial();
        ArmourManager.PlayOnDestructionSmoke();
        Helper.SFXManager.PlaySmokeEntry();

    }
    protected virtual void Update()
    {
        CheckState();
        stateManagers[(int)state].ExecuteUpdate();
    }
    protected virtual void FixedUpdate()
    {
        stateManagers[(int)state].ExecuteFixedUpdate();
    }
    void CheckState()
    {
        // Handle HP at 0
        if (HP <= 0 && state != enemyStates.Dead)
        {
            SetState(enemyStates.Dead);
            return;
        }

        switch (state)
        {
            case enemyStates.Dead:
            case enemyStates.Hurt:
            case enemyStates.Parried:
            case enemyStates.AttackingMelee:
            case enemyStates.AttackingRanged:
                break;
            default:
                // Handle other states
                if (onSpawnIdle || player.state == PlayerController.playerFSMMain.Dead)
                {
                    SetState(enemyStates.Idle);
                }
                else
                {
                    CheckRange(); // Proceed with range checks or other logic
                }
                break;
        }
        animator.SetInteger("State", (int)state);
    }
    void SetHPInitial()
    {
        int minHP = Mathf.Clamp(
            GameManager.instance.GetDifficultyModInt(difficultyCoefficeintMin),
            GameManager.EnemyMinHP, GameManager.EnemyMaxHP);
        int maxHP = Mathf.Clamp(
            minHP + GameManager.instance.GetDifficultyModInt(difficultyCoefficeintMax) + 1,
            GameManager.EnemyMinHP, GameManager.EnemyMaxHP);

        baseHP = Random.Range(minHP, maxHP + 1);
        HP = baseHP;
        ArmourManager.SetArmourForHP(baseHP);
        HPBarController.InitHP(baseHP);
    }
    public void SetState(enemyStates toSet)
    {
        state = toSet;
        //Debug.Log($"state: {state}");
        animator.SetInteger("State", (int)state);
        OnStateChanged.Invoke();
    }
    protected abstract void CheckRange();
    public void TakeDamage(string hitSource = "player")
    {
        damageSource = hitSource;
        if (state != enemyStates.Dead)
        {
            LoseHP();
        }
    }
    void LoseHP()
    {
        HP--;
        if (HP <= 0)
        {
            OnDeath.Invoke();
            if (damageSource == "player")
            {
                Helper.SFXManager.PlayDeadImpact();
            }
        }
        else
        {
            OnHurt.Invoke();
            if (damageSource == "player")
            {
                Helper.SFXManager.PlayHurtImpact();
            }
        }
        ArmourManager.GenerateHitParticles(HP);
        HPBarController.OnTakeDamage();
    }

    public List<GameObject> GetWeaponNames()
    {
        List<GameObject> activeWeapons = leftWeapons.Where(n => n.activeSelf == true).ToList();
        activeWeapons.AddRange(rightWeapons.Where(n => n.activeSelf == true).ToList());
        return activeWeapons;
    }
    public int GetHP()
    {
        return HP;
    }
    IEnumerator StopOnSpawnIdle()
    {
        yield return new WaitForSeconds(0.75f);
        onSpawnIdle = false;
    }
    public virtual void SetAnimatorParameters(int rWeaponIndex, int lWeaponIndex)
    {
        animator.SetInteger("State", (int)state);
        animator.SetInteger("L_Weapon", lWeaponIndex);
        animator.SetInteger("R_Weapon", rWeaponIndex);
    }
    protected void SetStateBasedOnNavMesh()
    {
        if (navMeshAgent.CanMove)
        {
            SetState(enemyStates.Moving);
        }
        else
        {
            SetState(enemyStates.Idle);
        }
    }
    protected bool ColliderIsOfLayer(Collider collider, string layerName)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer(layerName);
    }
    public void SetParentPool(EnemyPool parentPool) 
    {
        this.parentPool = parentPool;
    }
    public void ReturnToPool()
    {
        parentPool.ReturnToPool(gameObject);
    }
}