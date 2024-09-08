using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolsManager : MonoBehaviour
{
    [SerializeField] List<EnemyPool> pools;

    public EnemyPool GetEnemyPoolForPrefix(string prefix)
    {
        //gets name prefixes
        List<string> enemyPrefixes = EnemyInitializer.EnemyNamePrefixes;
        //if parameter prefix in name prefixes
        if (enemyPrefixes.Contains(prefix))
        {
            //return pool corresponding to prefix
            return pools[enemyPrefixes.IndexOf(prefix)];
        }
        //log for debugging
        Debug.LogWarning($"Prefix {prefix} not found!");
        //fallback
        return pools[0];
    }
}
