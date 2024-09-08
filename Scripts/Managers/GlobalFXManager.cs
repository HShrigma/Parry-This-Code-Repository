using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFXManager : MonoBehaviour
{
    [Header("Hit Lag Params")]
    [SerializeField] float hitLagOffset = 0f;
    [SerializeField] float hitLagSustain = .05f;
    bool isHitlagPlaying = false;
    [Header("UI Helpers")]
    public static List<Color> EnemyUIColors = new List<Color>()
    {
        new Color(0.2980392f,1f,1f), //melee
        new Color(0.2196079f,0.5058824f,0.6705883f), //elite melee
        new Color(1f,0.2980392f,0.2980392f), //gun
        new Color(0.6117647f,0.1882353f,0.1882353f), //elite gun
        new Color(1f,0.9960785f,0.2980392f), //uzi
        new Color(0.6117647f,0.6f,0.1882353f), //elite uzi
        new Color(0.2980392f,1f,0.2980392f), //grenade
        new Color(0.1882353f,0.6117647f,0.254902f), //elite grenade
        new Color(1f,1f,1f), //bazooka
        new Color(0.8f,0.2980392f,1f) //mixed
    };
    [Header("Camera Shake")]
    [SerializeField] CinemachineImpulseSource impulseSource;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHitLag(float offsetMod = 1f, float sustainMod = 1f)
    {
        StartCoroutine(HitLagInit(offsetMod, sustainMod));
    }

    IEnumerator HitLagInit(float offsetMod, float sustainMod)
    {
        if (!isHitlagPlaying)
        {
            isHitlagPlaying=true;
            yield return new WaitForSeconds(hitLagOffset * offsetMod);
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(hitLagSustain * sustainMod);
            Time.timeScale = 1f;
            isHitlagPlaying = false;
        }
    }

    public void ShakeCamGlobal(Vector3 dir, float amount = 1f) 
    {
        CameraShaker.ShakeCamera(impulseSource, dir, amount);
    }
}
