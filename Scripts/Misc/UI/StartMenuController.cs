using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;
    [SerializeField] ButtonColorTint startTint;
    [SerializeField] ButtonColorTint optionsTint;
    [SerializeField] ButtonColorTint quitTint;
    private void OnEnable()
    {
        SetSelectionDefault();
    }
    void SetSelectionDefault()
    {
        startButton.Select();
        startTint.SetSelectTint();
        optionsTint.SetDeselectTint();
        quitTint.SetDeselectTint();
    }
}
