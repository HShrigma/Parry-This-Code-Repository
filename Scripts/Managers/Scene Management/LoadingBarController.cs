using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour
{
    [SerializeField] Slider progressBar;
    [SerializeField] Image progressFillIMG;
    [SerializeField] Color startBarColor;
    [SerializeField] Color endBarColor;
    [SerializeField] List<string> sceneNames;

    public static LoadingBarController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        progressBar.value = 0;
        LoadScene(PersistentSceneManager.instance.SceneToLoad);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Shader warmup (80% progress)
        float shaderWarmupDuration = 5f; // Duration for shader warmup
        float elapsedTime = 0f;

        while (elapsedTime < shaderWarmupDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            float shaderProgress = Mathf.Clamp01(elapsedTime / shaderWarmupDuration);
            UpdateSlider(shaderProgress * 0.8f); // 80% of the total progress
        }

        // Scene loading (20% progress)
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        progressFillIMG.color = startBarColor;

        while (!operation.isDone)
        {
            // Scene loading progress
            float sceneProgress = Mathf.Clamp01(operation.progress / 0.9f); // Max 0.9 as progress cap
            UpdateSlider(0.8f + sceneProgress * 0.2f); // Add remaining 20%

            // When the scene is ready (90% loaded), allow activation
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void UpdateSlider(float progress)
    {
        progressBar.value = progress;
        progressFillIMG.color = Color.Lerp(startBarColor, endBarColor, progress);
    }
}