using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    public bool isEnabled;
    protected Enemy.enemyStates self_state;
    protected Enemy self;
    [SerializeField] protected EnemyStateHelper helper;
    protected GameManager gm;
    public virtual void SetStats()
    {
        gm = GameManager.instance;
        self = helper.self;
    }
    public abstract void ExecuteUpdate();
    public abstract void ExecuteFixedUpdate();
    public virtual void OnStateChanged()
    {

        if (self.state == self_state)
        {
            isEnabled = true;
        }
        else
        {
            isEnabled = false;
        }
    }
}
