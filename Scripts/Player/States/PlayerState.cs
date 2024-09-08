using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public bool isEnabled;
    protected PlayerController.playerFSMMain self_state;
    [SerializeField] protected PlayerStateHelper helper;
    protected abstract void Awake();
    public abstract void ExecuteUpdate();
    public abstract void ExecuteFixedUpdate();
    public virtual void OnStateChanged()
    {
        if (PlayerController.instance.state == self_state)
        {
            isEnabled = true;
        }
        else
        {
            isEnabled = false;
        }
    }
}
