using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] List<GameObject> menus = new List<GameObject>();
    [SerializeField] float tutorialTime;
    public UnityEvent OnTutorialTimeEnded;
    GameObject lastSelected;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(WaitForTutorialTime());
    }
    void Update()
    {
        HandleSelectedButtons();
    }
    public void StartGame()
    {
        PersistentSceneManager.instance.SceneToLoad = "SampleScene";
        SceneManager.LoadScene("LoadingScreen");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void SetActiveMenu(int index)
    {
        menus.ForEach(menu => menu.SetActive(false));
        menus[index].SetActive(true);
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

    IEnumerator WaitForTutorialTime()
    {
        yield return new WaitForSeconds(tutorialTime);
        OnTutorialTimeEnded.Invoke();
    }
}
