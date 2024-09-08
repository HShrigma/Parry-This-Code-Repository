using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerSlashVFXProcessor : MonoBehaviour
{

    [SerializeField] VisualEffect effect;
    [SerializeField] Transform effectTransform;
    [Header("Slash Parameters")]
    [SerializeField] List<float> effectLifetimes;
    [SerializeField] List<float> effectDirections;
    [SerializeField] List<Vector3> effectScales;
    [SerializeField] List<Vector3> effectRotations;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.transform.rotation.y > 0)
        {
            effectDirections[2] = 1;
            effectRotations[2] = new Vector3(effectRotations[2].x, 90f, effectRotations[2].z);
        }
        else
        {
            effectDirections[2] = -1;
            effectRotations[2] = new Vector3(effectRotations[2].x, -90f, effectRotations[2].z);
        }
    }

    public void PlaySlash(int attackCount)
    {
        effect.SendEvent("SingleStop");

        effect.SetFloat("Lifetime", effectLifetimes[attackCount]);
        effect.SetFloat("SlashDir", effectDirections[attackCount]);
        effectTransform.localScale = effectScales[attackCount];
        effect.SetVector3("InitRotation", effectRotations[attackCount]);
        effect.SendEvent("SinglePlay");
    }
}
