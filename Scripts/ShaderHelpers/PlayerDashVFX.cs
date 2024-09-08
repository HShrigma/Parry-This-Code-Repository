using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashVFX : MonoBehaviour
{
    [SerializeField] PlayerController self;
    VFXPool dash1Pool;
    VFXPool dash2Pool;
    void Awake()
    {
        dash1Pool = ObjectPoolManager.instance.VFXPoolsManager.PlayerDash1Pool;
        dash2Pool = ObjectPoolManager.instance.VFXPoolsManager.PlayerDash2Pool;
    }
    public void PlayDashStartVFX()
    {
        GameObject temp = dash1Pool.GetGameObjectTransform(self.transform.position,self.transform.rotation);
    }

    public void PlayDashEndVFX()
    {
        GameObject temp = dash2Pool.GetGameObjectTransform(self.transform.position);
    }

}
