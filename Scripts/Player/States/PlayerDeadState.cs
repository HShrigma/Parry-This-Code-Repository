using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeadState : PlayerState
{
    public UnityEvent OnPlayerDead;
    bool hasShook = false;
    [SerializeField] float timeToInvokeDead;
    protected override void Awake()
    {
        self_state = PlayerController.playerFSMMain.Dead;
        OnPlayerDead.AddListener(GameManager.instance.OnPlayerDeadHandler);
        OnPlayerDead.AddListener(UIManager.instance.OnPlayerDeadHandler);
    }
    public override void ExecuteFixedUpdate()
    {
    }

    public override void ExecuteUpdate()
    {
    }

    public override void OnStateChanged()
    {
        base.OnStateChanged();
        if(isEnabled)
        {
            helper.SetPlayerDamageSource();
            helper.DisableHurtBox();
            helper.SetDeadRB();
            if (!hasShook)
            {
                helper.InitCameraShake(4f);
                helper.InitHitLag();
                helper.SFXManager.PlayDead();
                StartCoroutine(InvokePlayerDeadForTime());
                hasShook = true;
            }
        }
    }
    IEnumerator InvokePlayerDeadForTime()
    {
        yield return new WaitForSeconds(timeToInvokeDead);
        OnPlayerDead.Invoke();
    }
}
