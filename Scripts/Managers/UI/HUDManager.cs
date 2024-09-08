using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("TextMeshPro")]
    [SerializeField] TextMeshProUGUI ScoreTMPro;
    [SerializeField] TextMeshProUGUI WavesTMPro;
    [Header("Off-Screen Indicators")]
    [SerializeField] GameObject offScreenPrefab;
    [SerializeField] EnemyManager enemyManager;
    List<GameObject> enemyList = new List<GameObject>();
    [Header("PlayerHP")]
    [SerializeField] List<Image> HPIcons;
    [SerializeField] List<ParticleSystem> HPParticles;
    [Header("CameraValues")]
    [SerializeField] RectTransform parentRect;
    [SerializeField] Camera UICam;
    int playerHP;
    void Start()
    {
        DrawScore();
        DrawWave();
        playerHP = PlayerController.instance.HP;
    }
    public void DrawScore()
    {
        ScoreTMPro.text = $"Score: {GameManager.instance.Score}";
    }

    public void DrawWave()
    {
        WavesTMPro.text = $"Wave {GameManager.instance.Wave}";
    }

    public void GetNewEnemies()
    {
        enemyList = enemyManager.Enemies;
        InstantiateIndicators();
    }

    void InstantiateIndicators()
    {
        foreach (GameObject enemy in enemyList)
        {
            var temp = Instantiate(offScreenPrefab, transform);
            temp.GetComponent<OffScreenIndicator>().SetParams(enemy.GetComponent<Enemy>(),parentRect,UICam);
        }
    }

    public void OnPlayerHPChanged()
    {
        int tempHP = PlayerController.instance.HP;
        HandleHPEnabled(tempHP);
    }

    void EnableHPIcon(int index)
    {
        HPIcons[index].enabled = true;
    }
    void DisableHPIcon(int index)
    {
        HPIcons[index].enabled = false;
        HPParticles[index].Play();
    }
    void HandleHPEnabled(int index)
    {
        //disable icons below index
        //0 HP -> Disable all
        //1 HP -> Disable HP2(mid) and HP3(top)
        //2 HP -> Disable HP3(top)
        //3 HP -> no loop
        for(int i = index; i < HPIcons.Count; i++)
        {
            if (HPIcons[i].enabled)
            {
                DisableHPIcon(i);
            }
        }
        //enable icons above index
        //0HP -> no loop
        //1HP -> enable HP1(bot)
        //2HP -> Enable HP1 and HP2
        //3HP -> Enable All
        for (int i = 0; i < index; i++)
        {
            if (!HPIcons[i].enabled)
            {
                EnableHPIcon(i);
            }
        }
    }
}
