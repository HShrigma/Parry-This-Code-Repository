using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource src;
    public void ShakeCamera(Vector3 dir, float shakeAmount = 1f)
    {
        src.GenerateImpulse(dir * shakeAmount);
    }
    public static void ShakeCamera(CinemachineImpulseSource source, Vector3 dir, float shakeAmount = 1f)
    {
        source.GenerateImpulse(dir * shakeAmount);
    }
}
