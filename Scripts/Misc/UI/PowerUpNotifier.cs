using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpNotifier : MonoBehaviour
{
    [SerializeField] GameObject scoreUpPrefab;
    [SerializeField] GameObject healthUpPrefab;
    [SerializeField] int scoreUpPoolSize;
    List<GameObject> scoreUpPool;
    List<GameObject> healthUpPool;
    Queue<bool> powerUpBuffer;
    bool isShowingPowerUp;

    void Start()
    {
        scoreUpPool = new List<GameObject>();
        healthUpPool = new List<GameObject>();
        powerUpBuffer = new Queue<bool>();

        for (int i = 0; i < scoreUpPoolSize; i++)
        {
            scoreUpPool.Add(Instantiate(scoreUpPrefab, transform));
            scoreUpPool[i].SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            healthUpPool.Add(Instantiate(healthUpPrefab, transform));
            healthUpPool[i].SetActive(false);
        }
    }

    void ShowScoreUp()
    {
        for (int i = 0; i < scoreUpPool.Count; i++)
        {
            if (scoreUpPool[i] != null && !scoreUpPool[i].activeSelf)
            {
                scoreUpPool[i].SetActive(true);
                return;
            }
        }
        Debug.Log("No available score up objects");
        scoreUpPool.Add(Instantiate(scoreUpPrefab, transform));
    }

    void ShowHealthUp()
    {
        for (int i = 0; i < healthUpPool.Count; i++)
        {
            if (healthUpPool[i] != null && !healthUpPool[i].activeSelf)
            {
                healthUpPool[i].SetActive(true);
                return;
            }
        }
        Debug.Log("No available health up objects");
        healthUpPool.Add(Instantiate(healthUpPrefab, transform));
    }

    public void ShowPowerupBuffered(bool scoreUp)
    {
        //add to buffer
        powerUpBuffer.Enqueue(scoreUp); 
        ProcessNextPowerUp();
    }

    void ProcessNextPowerUp()
    {
        if (!isShowingPowerUp && powerUpBuffer.Count > 0)
        {
            //get next power-up
            bool nextPowerUp = powerUpBuffer.Dequeue(); 
            StartCoroutine(ShowPowerUpAfterTime(nextPowerUp));
        }
    }

    IEnumerator ShowPowerUpAfterTime(bool scoreUp)
    {
        isShowingPowerUp = true;
        ShowHPOrScore(scoreUp);

        //set for animation duration
        yield return new WaitForSeconds(2f);

        isShowingPowerUp = false;
        //recursively calls the next power up in buffer
        ProcessNextPowerUp(); 
    }

    void ShowHPOrScore(bool scoreUp)
    {
        if (scoreUp)
        {
            ShowScoreUp();
        }
        else
        {
            ShowHealthUp();
        }
    }
}
