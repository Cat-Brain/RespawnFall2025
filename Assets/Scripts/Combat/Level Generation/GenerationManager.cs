using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct Level
{
    public GameObject prefab;
    public float height;
    public Vector2 spawnOffset;
    public List<int> validSpawnLevels;

    public readonly GameObject Spawn(Vector2 position, Transform parent = null)
    {
        return UnityEngine.Object.Instantiate(prefab, position + spawnOffset, Quaternion.identity, parent);
    }
}

public class GenerationManager : MonoBehaviour
{
    public GameManager gameManager;
    public EnemyManager enemySpawnManager;

    public float levelHeightOffset;
    public List<Level> levels;

    public List<(GameObject obj, Level level)> spawnedLevels = new();
    public Level currentLevel;
    public GameObject currentLevelObj;

    public void SpawnLevel(int level)
    {
        Level[] validLevels = levels.Where((potentialLevel) => potentialLevel.validSpawnLevels.Contains(level)).ToArray();
        currentLevel = validLevels[UnityEngine.Random.Range(0, validLevels.Length)];
        currentLevelObj = currentLevel.Spawn(Vector2.up * TotalSpawnedLevelHeight());
        spawnedLevels.Add((currentLevelObj, currentLevel));

        enemySpawnManager.LoadEnemiesInLevel(currentLevelObj);
    }

    public float TotalSpawnedLevelHeight()
    {
        float result = levelHeightOffset * (spawnedLevels.Count + 1);
        foreach ((GameObject obj, Level level) in spawnedLevels)
            result += level.height;
        return result;
    }
}
