using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Instance
    public static PlayerController instance;
    public int HP { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] PlayerParryBox parryBox;
    //GameObject hitbox;
    #region FSM
    public enum playerFSMMain
    {
        Idle,
        Moving,
        Attacking,
        Dashing,
        Hurt,
        Dead
    }
    [Header("States")]
    public playerFSMMain state;
    [SerializeField] List<PlayerState> stateManagers;
    public PlayerStateHelper Helper;
    #endregion
    public UnityEvent OnStateChanged;
    public string lastDamageSource;
    bool justHurt = false;

    public UnityEvent OnHPChanged;
    void Start()
    {
        state = playerFSMMain.Idle;
        HP = 3;
        OnHPChanged.AddListener(UIManager.instance.HudManager.OnPlayerHPChanged);
    }
    void FixedUpdate()
    {
        stateManagers[(int)state].ExecuteFixedUpdate();
    }

    void Update()
    {
        if(HP <=0 && state != playerFSMMain.Dead)
        {
            SetState(playerFSMMain.Dead);
        }
        SetStateOnInput();
        stateManagers[(int)state].ExecuteUpdate();
    }

    void SetStateOnInput()
    {
        //state detection based on input and priority
        //priority is as follows: Dead > Hurt > Dashing > Attacking > Idle,Movement
        if (state != playerFSMMain.Dead)
        {
            if (state != playerFSMMain.Hurt)
            {
                if (state != playerFSMMain.Dashing)
                {
                    if (state != playerFSMMain.Attacking)
                    {
                        SetIdleOrMoving();
                    }
                }
            }
        }
        animator.SetInteger("State", (int)state);
        OnStateChanged.Invoke();
    }
    public void ResetState()
    {
        SetIdleOrMoving();
        animator.SetInteger("State", (int)state);
        OnStateChanged.Invoke();
    }
    void SetIdleOrMoving()
    {
        //get magnitude of movement input vector
        float movementMagnitude = Helper.MoveInput.magnitude;
        //toggle idle and moving based on that magnitude
        //i.e. no movement key pressed = idle, else = moving
        if (movementMagnitude == 0f)
        {
            state = playerFSMMain.Idle;
        }
        else
        {
            state = playerFSMMain.Moving;
        }
    }
    #region Animation Methods
    public void SetState(playerFSMMain toSet)
    {
        if (state <= toSet)
        {
            state = toSet;
        }
    }
    #endregion
    public void SetAnimatorAttackCounter(int value)
    {
        animator.SetInteger("AttackCounter", value);
    }
    public void SetAnimatorDamageSource(int value)
    {
        animator.SetInteger("DamageSource", value);
    }
    public void TakeDamage(string damageSource="melee")
    {
        if (!justHurt)
        {
            justHurt = true;
            StartCoroutine(SetJustHurtFalse());
            if (state != playerFSMMain.Hurt && state != playerFSMMain.Dead)
            {
                HP--;
                lastDamageSource = damageSource;
                OnHPChanged.Invoke();

                if (HP <= 0)
                {
                    state = playerFSMMain.Dead;
                }
                else
                {
                    state = playerFSMMain.Hurt;
                }
            }
        }
    }

    IEnumerator SetJustHurtFalse()
    {
        yield return new WaitForSeconds(.5f);
        justHurt = false;
    }

    public void AddHP()
    {
        HP++;
    }
    public void SetParryBox()
    {
        if (!parryBox.IsColliderActive())
        {
            parryBox.EnableForTime();
        }
    }
}