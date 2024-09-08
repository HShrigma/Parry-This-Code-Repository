using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyWeaponVFXManager : MonoBehaviour
{
    VFXPoolsManager poolsManager;
    VFXPool muzzleFlashPool;
    VFXPool grenadeLaunchPool;
    VFXPool rocketLaunchPool;
    void GetPoolsManager()
    {
        if (poolsManager == null)
        {
            poolsManager = ObjectPoolManager.instance.VFXPoolsManager;
        }
    }
    void PlayMuzzleFlash(Transform t)
    {
        if(muzzleFlashPool == null)
        {
            muzzleFlashPool = poolsManager.MuzzleFlashPool;
        }
        muzzleFlashPool.GetGameObjectTransform(t.position, t.rotation);
    }
    void PlayGrenadeLaunch(Transform t)
    {
        if (grenadeLaunchPool == null)
        {
            grenadeLaunchPool = poolsManager.GrenadeLaunchPool;
        }
        grenadeLaunchPool.GetGameObjectTransform(t.position, t.rotation);
    }

    void PlayBazookaLaunch(Transform t)
    {
        if (rocketLaunchPool == null)
        {
            rocketLaunchPool = poolsManager.RocketLaunchPool;
        }
        rocketLaunchPool.GetGameObjectTransform(t.position, t.rotation);
    }
    public void PlayRangedVFXForWeaponIndex(int index, Transform t)
    {
        GetPoolsManager();
        switch (index)
        {
            case 2:
            case 3:
                PlayMuzzleFlash(t);
                break;
            case 4:
                PlayGrenadeLaunch(t);
                break;
            case 5:
                PlayBazookaLaunch(t);
                break;
            default:
                Debug.LogError($"No such weapon index: {index}");
                break;
        }
    }
}
