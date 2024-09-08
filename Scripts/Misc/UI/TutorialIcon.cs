using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialIcon : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] List<Vector2> positions;
    void OnEnable()
    {
        img.canvasRenderer.SetAlpha(0f);
    }

    public void SetActive()
    {
        img.CrossFadeAlpha(1f,.25f,true);
        img.canvasRenderer.SetAlpha(1f);
    }
    public void SetPos(int index)
    {
        rectTransform.anchoredPosition = positions[index];
    }
}
