using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUpTextController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TMPro;
    [SerializeField] RectTransform rect;
    [SerializeField] float originY;
    [SerializeField] float maxY;
    [SerializeField] float fadeTime;
    [SerializeField] bool isScore;
    private void OnEnable()
    {
        if (isScore)
        {
            TMPro.text = $"+{GameManager.instance.BonusScore}";
        }
        TMPro.canvasRenderer.SetAlpha(1f);
        SetRectTransformY(originY);
        StartCoroutine(AnimatePowerUpText());
    }
    IEnumerator AnimatePowerUpText()
    {
        TMPro.CrossFadeAlpha(0f, fadeTime*1.5f, true);
        float progress = 0f;
        while (rect.anchoredPosition.y < maxY)
        {
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime / fadeTime;
            SetRectTransformY(Mathf.Lerp(originY, maxY, progress));
        }
        yield return new WaitForSeconds(fadeTime);
        gameObject.SetActive(false);
    }

    void SetRectTransformY(float value)
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, value);
    }
}
