using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHelper : MonoBehaviour
{
    public static Vector3 SetDirTowardsPlayer(Transform player, Transform obj, float maxOffset, float recoil)
    {
        //init direction
        Vector3 dirToPlayer = player.position - obj.position;
        dirToPlayer.Normalize();
        //apply recoil (slight random deviation)
        Vector3 recoilOffset = new Vector3(
            Random.Range(-recoil, recoil),
            Random.Range(-recoil, recoil),
            Random.Range(-recoil, recoil)
        );

        //add recoilOffset to direction and lerp to max offset range
        dirToPlayer += recoilOffset;
        Vector3 finalDir = Vector3.Lerp(obj.forward, dirToPlayer, maxOffset);
        return finalDir.normalized;
    }
}
