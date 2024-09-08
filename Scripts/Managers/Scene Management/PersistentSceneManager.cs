using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string SceneToLoad;
}
