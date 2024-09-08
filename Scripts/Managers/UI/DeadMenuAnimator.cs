using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeadMenuAnimator : MonoBehaviour
{
    [Header("Background Fade")]
    [SerializeField] Image BackgroundTint;
    [SerializeField] float BGStartAlpha;
    [SerializeField] float BGEndAlpha;
    [SerializeField] float timeToLerpBackGroundTint;
    [Header("Foreground Fade")]
    [SerializeField] Image ForegroundTint;
    [SerializeField] float FGStartAlpha;
    [SerializeField] float FGEndAlpha;
    [SerializeField] float timeToLerpForeGroundTint;
    [Header("TextMeshPro")]
    [SerializeField] List<TextMeshProUGUI> TMProList;
    [SerializeField] float TMProLerpTime;
    [Header("Buttons and Buttons TextMeshPro")]
    [SerializeField] List<Button> buttons;
    [SerializeField] List<TextMeshProUGUI> buttonTMPRoList;
    public UnityEvent OnDeadMenuLoaded;

    bool newHighScore = false;
    bool newMaxWave = false;
    string prefixNew = "New ";
    void Start()
    {
        FadeColors();
        SetTMPRoTexts();
    }

    void FadeColors()
    {
        SetBaseColorValues();
        StartCoroutine(FadeColorsForTime());
    }

    IEnumerator FadeColorsForTime()
    {
        StartCoroutine(FadeImageAlphaForTime(BackgroundTint, BGEndAlpha, timeToLerpBackGroundTint));
        yield return new WaitForSeconds(timeToLerpBackGroundTint);
        StartCoroutine(FadeImageAlphaForTime(ForegroundTint, FGEndAlpha, timeToLerpForeGroundTint));
        yield return new WaitForSeconds(timeToLerpForeGroundTint);
        StartCoroutine(FadeAllTMProsForTime(TMProLerpTime));
        yield return new WaitForSeconds(TMProLerpTime * TMProList.Count);
        StartCoroutine(FadeButtons());
        yield return new WaitForSeconds(TMProLerpTime * buttons.Count);
        OnDeadMenuLoaded.Invoke();
    }
    IEnumerator FadeImageAlphaForTime(Image image, float endAlpha, float fadeTime)
    {
        image.CrossFadeAlpha(endAlpha, fadeTime, true);
        yield return new WaitForSeconds(fadeTime);
    }
    Color LerpColor(Color from, Color to, float progress)
    {
        return Color.Lerp(from, to, progress);
    }

    IEnumerator FadeAllTMProsForTime(float lerpTime)
    {
        foreach (var TMPro in TMProList)
        {
            StartCoroutine(FadeTMProForTime(TMPro, lerpTime));
            yield return new WaitForSeconds(lerpTime);
        }
    }
    IEnumerator FadeTMProForTime(TextMeshProUGUI TMPro, float fadeTime)
    {
        TMPro.CrossFadeAlpha(1f, fadeTime, true);
        yield return new WaitForSeconds(fadeTime);
    }
    IEnumerator FadeButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            StartCoroutine(FadeTMProForTime(buttonTMPRoList[i], TMProLerpTime));
            StartCoroutine(FadeButtonFill(buttons[i], TMProLerpTime));
            yield return new WaitForSeconds(TMProLerpTime);
        }
    }

    IEnumerator FadeButtonFill(Button button, float fadeTime)
    {
        float progress = 0f;
        while (button.image.fillAmount != 1f)
        {
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime / fadeTime;
            button.image.fillAmount = Mathf.Lerp(0f, 1f, progress);
        }
    }

    void SetScoreText()
    {
        TMProList[1].text = $"Score: {GameManager.instance.Score}";
    }
    void SetWaveText()
    {
        TMProList[3].text = $"Wave: {GameManager.instance.Wave}";
    }
    void SetHighScoreText()
    {
        string text = $"High Score: {GameManager.instance.HighScore}";
        if (newHighScore)
        {
            text = string.Concat(prefixNew, text);
        }
        TMProList[2].text = text;
    }
    void SetMaxWaveText()
    {
        string text = $"Max Wave: {GameManager.instance.MaxWave}";
        if (newMaxWave)
        {
            text = string.Concat(prefixNew,text);
        }
        TMProList[4].text = text;
    }

    void SetBaseColorValues()
    {
        BackgroundTint.canvasRenderer.SetAlpha(0f);
        BackgroundTint.canvasRenderer.SetAlpha(0f);
        TMProList.ForEach(t => t.canvasRenderer.SetAlpha(0f));
        buttons.ForEach(b => b.image.fillAmount = 0f);
        buttonTMPRoList.ForEach(t => t.canvasRenderer.SetAlpha(0f));
    }
    void SetTMPRoTexts()
    {
        SetScoreText();
        SetHighScoreText();
        SetWaveText();
        SetMaxWaveText();
    }
    public void OnNewHighScoreHandler()
    {
        newHighScore = true;
    }
    public void OnNewMaxWaveHandler()
    {
        newMaxWave = true;
    }

}
