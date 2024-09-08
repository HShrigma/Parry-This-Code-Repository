using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] float enemySpawnTime;
    [SerializeField] float SpawnRangeCoefficient;
    [SerializeField] List<Transform> spawnPoints;
    EnemyPoolsManager poolManager;
    [HideInInspector] public List<GameObject> Enemies { get; private set; }
    [Header("References")]
    [SerializeField] EnemyInitializer initializer;

    uint counter = 0;
    public UnityEvent OnNoEnemies;
    public UnityEvent OnEnemiesSpawned;
    bool isSpawnDelayed = false;
    void Start()
    {
        Enemies = new List<GameObject>();
        poolManager = ObjectPoolManager.instance.EnemyPools;
        OnNoEnemies.AddListener(GameManager.instance.IncrementWave);
    }

    void Update()
    {
        CheckDead();
    }
    public int GetSpawnPointsCount()
    {
        return spawnPoints.Count;
    }

    List<int> GenAllowedSpawnPoints()
    {
        //get random number of enemies to spawn based on wave
        int spawnNum = GetSpawnNumberForDifficulty();
        List<int> spawnIndexes = new List<int>();
        //generate spawn indexes
        while (spawnNum > 0)
        {
            //get random index
            int index = Random.Range(0, spawnPoints.Count);
            //track shifts
            int indexShiftCount = 0;
            while (spawnIndexes.Contains(index))
            {
                //increment index with loop
                index = (index + 1) % spawnPoints.Count;
                //raise shift track
                indexShiftCount++;
                if(indexShiftCount == spawnPoints.Count)
                {
                    //return spawn indexes on no available spawn points
                    return spawnIndexes;
                }
            }
            //Add index to spawn indexes
            spawnIndexes.Add(index);
            //decrease spawn numbers left
            spawnNum--;
        }
        return spawnIndexes;
    }
    int GetSpawnNumberForDifficulty()
    {
        //get range min and max
        //  min = wave
        //  max = min + difficulty mod
        int spawnNumMin = GameManager.instance.Wave;
        int spawnNumMax = spawnNumMin + GameManager.instance.GetDifficultyModInt(SpawnRangeCoefficient) + 1;
        //clamp range limits as fallback
        spawnNumMin = Mathf.Clamp(spawnNumMin, 1, spawnPoints.Count);
        spawnNumMax = Mathf.Clamp(spawnNumMax, 1, spawnPoints.Count);
        //return random number in range
        return Random.Range(spawnNumMin, spawnNumMax);
    }
    IEnumerator SpawnEnemies()
    {
        List<int> indexes = GenAllowedSpawnPoints();
        for (int i = 0; i < indexes.Count; i++)
        {
            int index = indexes[i];
            int[] weaponIndexes;
            string prefix;
            (prefix, weaponIndexes) = initializer.GetPrefixAndWeaponIndexes();

            //GameObject enemy = Instantiate(prefab, spawnPoints[index].position, Quaternion.identity);
            EnemyPool pool = poolManager.GetEnemyPoolForPrefix(prefix);
            GameObject enemy = pool.GetGameObjectTransform(spawnPoints[index].position,Quaternion.identity);
            GetUniqueEnemyName(enemy);
            Enemies.Add(enemy);

            Enemies[Enemies.Count - 1].GetComponent<Enemy>().SetAnimatorParameters(weaponIndexes[0], weaponIndexes[1]);
            counter++;
            yield return new WaitForSeconds(0.1f);
        }
        OnEnemiesSpawned.Invoke();
    }
    void  GetUniqueEnemyName(GameObject enemy)
    {
        enemy.name = enemy.name + counter.ToString();
        while (Enemies.Select(n => n.name).Contains(enemy.name))
        {
            enemy.name += Random.Range(0, 10).ToString();
        }
    }
    void CheckDead()
    {
        Enemies.RemoveAll(e => e == null || !e.activeSelf);
        if (Enemies.Count == 0 && !isSpawnDelayed)
        {
            OnNoEnemies.Invoke();
        }
    }

    IEnumerator SpawnEnemiesAfterTime()
    {
        yield return new WaitForSeconds(enemySpawnTime);
        StartCoroutine(SpawnEnemies());
        isSpawnDelayed = false;
    }

    public void SpawnEnemiesDelayed()
    {
        isSpawnDelayed = true;
        StartCoroutine(SpawnEnemiesAfterTime());
    }
}