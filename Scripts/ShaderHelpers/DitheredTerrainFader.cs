using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitheredTerrainFader : MonoBehaviour
{
    public enum faderState
    {
        opaque,
        faded,
        fading
    }
    public faderState State;
    Material mat;
    float defaultAlpha = 1f;
    [Header("Fade Parameters")]
    [SerializeField] float targetAlpha = 0.3f;
    [SerializeField] float duration;
    [SerializeField] Renderer parentRenderer;
    public bool Obscuring = false;

    void Start()
    {
        State = faderState.opaque;
        mat = parentRenderer.material;
        SetShaderAlpha(defaultAlpha);
    }
    IEnumerator FadeToValue(float targetAlpha,float duration)
    {
        float startAlpha = mat.color.a;
        float elapsedTime = 0f;

        State = faderState.fading;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            SetShaderAlpha(alpha);
            //wait until the next frame
            yield return null; 
        }

        SetShaderAlpha(targetAlpha);
        State = targetAlpha == defaultAlpha ? faderState.opaque : faderState.faded;
    }

    public void Fade()
    {
        if (State == faderState.opaque)
        {
            StartCoroutine(FadeToValue(targetAlpha,duration));
        }
    }

    public void Unfade()
    {
        if (State == faderState.faded)
        {
            StartCoroutine(FadeToValue(defaultAlpha,duration));
        }

    }

    void SetShaderAlpha(float value)
    {
        mat.SetFloat("_AlphaAlbedo",value);
    }
}

