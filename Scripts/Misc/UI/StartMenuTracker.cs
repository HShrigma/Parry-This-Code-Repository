using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartMenuTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI WaveText;
    void Start()
    {
        int highScore = PlayerPrefsManager.GetHighScore();
        if (highScore == 0)
        {
            ScoreText.text = string.Empty;
            WaveText.text = string.Empty;
        }
        else
        {
            int maxWave = PlayerPrefsManager.GetMaxWave();
            ScoreText.text += highScore.ToString();
            WaveText.text += maxWave.ToString();
        }
    }
}
