using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadMenuManager : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button menuButton;
    [SerializeField] ButtonColorTint restartTint;
    [SerializeField] ButtonColorTint menuTint;
    bool active;
    GameObject lastSelected;
    [Header("Tutorial Icon Parameters")]
    [SerializeField] float timeUntilIcon;
    public UnityEvent onTutorialTimeEnded;
    void Awake()
    {
        active = false;
    }
    void Start()
    {

    }
    void Update()
    {
        if (active)
        {
            HandleSelectedButtons();
        }
    }

    public void OnMenuLoaded()
    {
        active = true;
        restartButton.Select();
        restartTint.SetSelectTint();
        lastSelected = restartButton.gameObject;
        StartCoroutine(SetTutorialAfterTime());
    }

    public void Restart()
    {
        if (active)
        {
            PersistentSceneManager.instance.SceneToLoad = "SampleScene";
            SceneManager.LoadScene("LoadingScreen");
        }
    }

    public void GoToMainMenu()
    {
        if (active)
        {
            PersistentSceneManager.instance.SceneToLoad = "MainMenu";
            SceneManager.LoadScene("LoadingScreen");
        }
    }
    /*in case of deselecting all buttons (i.e. on Mouse Click), returns to last selection
used in events*/
    void HandleSelectedButtons()
    {
        if (AnyButtonIsSelected())
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }
    bool AnyButtonIsSelected()
    {
        return EventSystem.current.currentSelectedGameObject != null;
    }

    IEnumerator SetTutorialAfterTime()
    {
        yield return new WaitForSeconds(timeUntilIcon);
        onTutorialTimeEnded.Invoke();
    }
}
