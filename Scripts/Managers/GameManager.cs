using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] EnemyManager enemyManager;
    public GlobalFXManager GFXManager;
    public ProjectileSFXManager ProjectileSoundFXManager;
    public PlayerSFXManager PlayerSoundFXManager;
    public EnemySFXManager EnemySoundFXManager;
    public static GameManager instance;
    public static int EnemyMinHP = 1;
    public static int EnemyMaxHP = 4;
    public int Score { get; private set; }
    public int Wave { get; private set; }

    public int HighScore { get; private set; }

    public int MaxWave { get; private set; }
    [Header("PowerUps")]
    [SerializeField] int scoreForPowerUp;
    int scoreCounter;
    public int BonusScore;

    public UnityEvent OnWaveChanged;
    public UnityEvent OnScoreChanged;
    public UnityEvent OnNewHighScore;
    public UnityEvent OnNewMaxWave;
    public UnityEvent OnScoreUp;
    public UnityEvent OnHealthUp;

    bool noClear = true;
    bool playerIsAlive = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Score = 0;
        scoreCounter = 0;
        Wave = 0;
        HighScore = PlayerPrefsManager.GetHighScore();
        MaxWave = PlayerPrefsManager.GetMaxWave();

    }

    public void IncrementWave()
    {
        if (playerIsAlive)
        {
            Wave++;
            if (noClear)
            {
                noClear = false;
            }
            else
            {
                IncrementScore("clear");
            }
            OnWaveChanged.Invoke();
        }
    }

    public void IncrementScore(string type = "parry")
    {
        int value = 0;
        switch (type)
        {
            case "parry":
                value = 15;
                break;
            case "hurt":
                value = 10;
                break;
            case "dead":
                value = 20;
                break;
            case "clear":
                value = 50;
                break;
            default:
                Debug.LogError($"No such type: {type}");
                break;
        }
        Score += value;
        HandleScoreForPowerUp(value);
        OnScoreChanged.Invoke();
    }

    public int GetDifficultyModInt(float coefficient = 1f)
    {
        return Mathf.RoundToInt(GameManager.instance.Wave / (enemyManager.GetSpawnPointsCount() * coefficient));
    }

    public int GetTestDifficultyModInt(int wave, float coefficient = 1f)
    {
        return Mathf.RoundToInt(wave / (enemyManager.GetSpawnPointsCount() * coefficient));

    }

    public void OnPlayerDeadHandler()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefsManager.SetHighScore(Score);
            OnNewHighScore.Invoke();
        }
        if (Wave > MaxWave)
        {
            MaxWave = Wave;
            PlayerPrefsManager.SetMaxWave(Wave);
            OnNewMaxWave.Invoke();
        }
        playerIsAlive = false;
    }

    void HandleScoreForPowerUp(int toAdd)
    {
        scoreCounter += toAdd;
        if (scoreCounter >= scoreForPowerUp)
        {
            if (PlayerController.instance.HP >= 3)
            {
                Score += BonusScore;
                OnScoreUp.Invoke();
            }
            else
            {
                PlayerController.instance.AddHP();
                OnHealthUp.Invoke();
            }
            scoreCounter -= scoreForPowerUp;
            if (scoreCounter > 0)
            {
                HandleScoreForPowerUp(0);
            }
        }
    }
}
