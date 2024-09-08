using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXFlash : MonoBehaviour
{
    [Header("Invulnerability FX")]
    bool invulnerable;
    [SerializeField] PlayerStateHelper playerStateHelper;
    public enum flashState
    {
        flashingUp,
        flashingDown,
        notFlashing
    }
    flashState flash = flashState.notFlashing;
    [SerializeField] Material flashMaterial;
    [SerializeField, Range(0, 1)] float maxFlashAlpha = 0.7f;
    [SerializeField, Range(0, 1)] float minFlashAlpha = 0.1f;
    [SerializeField, Range(0, 1)] float flashAnimationSpeed = 0.1f;
    [SerializeField, Range(0, 1)] float flashRate = 0.05f;
    float desiredAlpha = 0f;

    void Start()
    {
        flashMaterial.color = new Color(flashMaterial.color.r, flashMaterial.color.g, flashMaterial.color.b, 0f);
    }

    void Update()
    {
        
        if (playerStateHelper.Invulnerable)
        {
            if (flash == flashState.notFlashing)
            {
                StartCoroutine(DamageFlashAnimation());
            }
        }
        else
        {
            desiredAlpha = 0f;
        }

        flashMaterial.color = new Color(flashMaterial.color.r, flashMaterial.color.g, flashMaterial.color.b, desiredAlpha);
    }

    IEnumerator DamageFlashAnimation()
    {
        flash = flashState.flashingUp;
        while (invulnerable)
        {
            yield return new WaitForSeconds(flashAnimationSpeed);
            if (desiredAlpha <= minFlashAlpha)
            {
                flash = flashState.flashingUp;
            }
            if (desiredAlpha >= maxFlashAlpha)
            {
                flash = flashState.flashingDown;
            }
            switch (flash)
            {
                case flashState.flashingUp:
                    desiredAlpha += flashRate;
                    break;
                case flashState.flashingDown:
                    desiredAlpha -= flashRate;
                    break;
            }

        }
        flash = flashState.notFlashing;
    }
}
