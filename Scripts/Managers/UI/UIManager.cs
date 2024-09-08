using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            SetDeadMenu(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public HUDManager HudManager;
    [SerializeField] DeadMenuManager deadMenuAnimator;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetDeadMenu(bool deadOn)
    {
        HudManager.gameObject.SetActive(!deadOn);
        deadMenuAnimator.gameObject.SetActive(deadOn);
    }
    public void OnPlayerDeadHandler()
    {
        SetDeadMenu(true);
    }
}
