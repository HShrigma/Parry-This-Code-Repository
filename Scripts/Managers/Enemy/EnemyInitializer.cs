using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyInitializer : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] EnemyManager enemyManager;

    //key = wave, value = maxIndex
    Dictionary<int, int> waveMaxIndex = new Dictionary<int, int>()
    {
        { 1, 1 },
        { 2, 2 },
        { 5, 3 },
        { 8, 4 },
        { 13, 5 }
    };
    public static List<string> EnemyNamePrefixes = new List<string>()
    {
        "Melee",
        "EliteMelee",
        "Gun",
        "EliteGun",
        "Uzi",
        "EliteUzi",
        "Grenade",
        "EliteGrenade",
        "Bazooka"
    };
    Dictionary<int, string> weaponIndexToPrefix = new Dictionary<int, string>()
    {
        { 1, "Melee" },
        { 2, "Gun" },
        { 3, "Uzi" },
        { 4, "Grenade" },
        { 5, "Bazooka" }
    };
    //TESTING
    [Header("Testing")]
    [SerializeField] int maxIndexTesting;
    int[] GenerateWeaponIndexes()
    {
        int[] indexes = new int[2];
        //get max weapon index based on wave        
        int maxRWeaponIndex = GetMaxRWeaponIndex();

        //search through names
        Dictionary<string, int> uniqueNameCounts = GetUniqueNameCounts();
        //get dynamic ratios
        Dictionary<int, float> spawnRatios = CalculateDynamicRatios(uniqueNameCounts);
        //get weighted random index
        indexes[0] = GetWeightedRandomIndex(maxRWeaponIndex, spawnRatios, uniqueNameCounts);
        indexes[1] = 0;
        //has LWeapon Check
        if (Random.Range(0f, 100f) <= 20f && indexes[0] != 5)
        {
            indexes[1] = indexes[0];
        }
        //[0]-right, [1]-left
        //[0]/[1] = 0 = none
        //[0]/[1] = 1 = knife
        //[0]/[1] = 2 = gun
        //[0]/[1] = 3 = uzi
        //[0]/[1] = 4 = grenade launcher
        //[0] = 5 = rocket Launcher
        return indexes;
    }

    int GetMaxRWeaponIndex()
    {
        //set allowed spawn index based on wave
        int wave = GameManager.instance.Wave;
        int maxSpawnIndex = 0;
        foreach (var kvp in waveMaxIndex)
        {
            if (wave >= kvp.Key)
            {
                maxSpawnIndex = kvp.Value;
            }
        }
        maxSpawnIndex = Mathf.Clamp(maxSpawnIndex, 1, 5);
        return maxSpawnIndex;
    }

    Dictionary<string, int> GetUniqueNameCounts()
    {
        //get each occurence of each enemy type
        Dictionary<string, int> uniqueNames = new Dictionary<string, int>();
        foreach (string enemyName in enemyManager.Enemies.Select(n => n.name))
        {
            //get unique name from prefixes
            string name = BuildUniqueName(enemyName);
            if (uniqueNames.ContainsKey(name))
            {
                uniqueNames[name]++;
            }
            else
            {
                uniqueNames.Add(name, 1);
            }
        }
        return uniqueNames;
    }

    string BuildUniqueName(string name)
    {
        foreach (string enemyPrefix in EnemyNamePrefixes)
        {
            if (name.StartsWith(enemyPrefix))
            {
                //return first matching name in prefixes
                return enemyPrefix;
            }
        }

        //fallback on faulty name
        Debug.LogWarning($"No Unique Name Found for \"{name}\"");
        return EnemyNamePrefixes[0];
    }
    Dictionary<int, float> CalculateDynamicRatios(Dictionary<string, int> uniqueEnemyCounts)
    {
        //get the total count
        int totalCount = uniqueEnemyCounts.Values.Sum();
        //initialize new array to hold spawn ratios for each index
        Dictionary<int, float> spawnRatios = new Dictionary<int, float>();

        //calculate the inverse proportional ratio for each weapon index
        foreach (var kvp in weaponIndexToPrefix)
        {
            string prefix = kvp.Value;
            int count = 0;
            if (uniqueEnemyCounts.ContainsKey(prefix))
            {
                count = uniqueEnemyCounts[prefix];
            }

            //calculate ratio: inverse of the count to prefer less frequent enemies
            float ratio = (float)(totalCount - count + 1) / totalCount;

            //store ratio associated with the weapon index
            spawnRatios[kvp.Key] = ratio;
        }

        float sumRatios = spawnRatios.Values.Sum();
        foreach (var key in spawnRatios.Keys.ToList())
        {
            spawnRatios[key] /= sumRatios;
        }

        return spawnRatios;
    }
    int GetWeightedRandomIndex(int maxRWeaponIndex, Dictionary<int, float> spawnRatios, Dictionary<string, int> uniqueEnemyCounts)
    {
        //get random value in range 0-1
        float rand = Random.value;
        //init cumulative ratio
        float cumulative = 0f;
        //if current unqiue enemies does not include all possible enemy spawns
        if (uniqueEnemyCounts.Count < maxRWeaponIndex)
        {
            //check against weaponIndexToPrefix to get unspawned index
            for (int i = 1; i <= maxRWeaponIndex; i++)
            {
                string prefix = weaponIndexToPrefix[i];
                if (!uniqueEnemyCounts.ContainsKey(prefix))
                {
                    //return the first unspawned allowed index
                    return i;
                }
            }
        }
        //loop through allowed indexes
        for (int i = 1; i <= maxRWeaponIndex; i++)
        {
            //get actual spawn with index
            if (spawnRatios.ContainsKey(i))
            {
                //add ratio to cumulative
                cumulative += spawnRatios[i];
                if (rand <= cumulative)
                {
                    //return index if not present enough.
                    //in this case - return index if ratio is smaller than cumulative ratio
                    return i;
                }
            }
        }
        //Debug.LogWarning("No appropriate random value found");
        return 1; // fallback, should not happen
    }
    string GetPrefix(int[] indexes)
    {
        if (indexes[1] != 0)
        {
            switch (indexes[1])
            {
                case 1:
                    return EnemyNamePrefixes[1];
                case 2:
                    return EnemyNamePrefixes[3];
                case 3:
                    return EnemyNamePrefixes[5];
                case 4:
                    return EnemyNamePrefixes[7];
                default:
                    Debug.LogError($"No such index for two handing: {indexes[0]}");
                    return EnemyNamePrefixes[indexes[1]];
            }
        }

        switch (indexes[0])
        {
            case 1:
                return EnemyNamePrefixes[0];
            case 2:
                return EnemyNamePrefixes[2];
            case 3:
                return EnemyNamePrefixes[4];
            case 4:
                return EnemyNamePrefixes[6];
            default:
                return EnemyNamePrefixes[8];
        }
    }
    public (string, int[]) GetPrefixAndWeaponIndexes()
    {
        int[] indexes = GenerateWeaponIndexes();
        string prefix = GetPrefix(indexes);
        return (prefix, indexes);
    }
}
