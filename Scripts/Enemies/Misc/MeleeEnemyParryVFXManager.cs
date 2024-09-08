using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class MeleeEnemyParryVFXManager : MonoBehaviour
{
    int weaponCount;
    [SerializeField] GameObject parryEffectObj;
    [SerializeField] float camShakeAmount;
    [SerializeField] CameraShaker camShaker;
    VFXPool parryPool;
    Vector3 impactPoint;


    public void ReceiveWeapons(List<GameObject> values)
    {
        weaponCount = values.Where(n => n.name.Contains("Knife")).Count();
    }

    public void PlayParryVFX()
    {
        if (parryPool == null)
        {
            parryPool = ObjectPoolManager.instance.VFXPoolsManager.GetParryPoolForIndex(0);
        }
        for (int i = 0; i < weaponCount; i++)
        {
            parryPool.GetGameObjectTransform(this.impactPoint, Quaternion.identity);
        }
        camShaker.ShakeCamera(Vector3.one, camShakeAmount);
    }

    public void GetImpactPoint(Vector3 value)
    {
        impactPoint = value;
        impactPoint.y = Random.Range(2f, 3f);
    }
}
