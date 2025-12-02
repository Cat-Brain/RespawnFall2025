using LDtkUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public struct EnemyTierData
{
    public int tier, typeCount, totalQuantity;
}

[Serializable]
public struct Wave
{
    public List<EnemyTierData> data;
}

[Serializable]
public struct LevelData
{
    public List<Wave> waves;
}

public class EnemyManager : MonoBehaviour
{
    public GameManager gameManager;
    public GenerationManager generationManager;
    public GameObject smokePrefab;

    public float waveSpawnDelay;
    public Vector2 smokeOffset;
    public List<LevelData> levelData;

    public List<Enemy> enemies;
    public int currentLevel;
    public int currentWave;
    public List<EnemySpawnPosition> spawnPositions;

    public void LoadEnemiesInLevel(GameObject levelParent, int level)
    {
        currentLevel = level;
        spawnPositions = levelParent.GetComponentsInChildren<EnemySpawnPosition>().ToList();

        if (spawnPositions.Count <= 0)
        {
            Debug.LogWarning("Missing Enemy Spawn Postions!!!");
            return;
        }

        currentWave = 0;
        gameManager.inCombat = SpawnWave();
    }

    public bool SpawnWave()
    {
        if (currentWave >= levelData[currentLevel].waves.Count)
            return false;

        bool firstWave = currentWave == 0;

        IEnumerable<Enemy> validEnemies = Enumerable.Empty<Enemy>();
        foreach (EnemySpawnPosition position in spawnPositions)
            validEnemies = validEnemies.Union(position.validSpawns);

        List<EnemySpawnPosition> spawnPositionsClone = new (spawnPositions);

        foreach (EnemyTierData tierData in levelData[currentLevel].waves[currentWave].data)
        {
            List<Enemy> enemiesInTier = validEnemies.Where((enemy) => enemy.tier == tierData.tier).ToList();
            Enemy[] enemiesToSpawn = new Enemy[tierData.typeCount];
            for (int i = 0; i < tierData.typeCount; i++)
            {
                int index = UnityEngine.Random.Range(0, enemiesInTier.Count);
                enemiesToSpawn[i] = enemiesInTier[index];
                enemiesInTier.RemoveAt(index);
            }

            for (int i = 0; i < tierData.totalQuantity; i++)
            {
                Enemy type = enemiesToSpawn[UnityEngine.Random.Range(0, tierData.typeCount)];
                List<EnemySpawnPosition> validSpawns =
                    spawnPositionsClone.Where((position) =>
                    CanSpawnAt(type, position.gridPos, spawnPositionsClone)).ToList();
                Vector2Int spawnPosition = validSpawns[UnityEngine.Random.Range(0, validSpawns.Count)].gridPos;

                spawnPositionsClone.RemoveAll((position) =>
                    position.gridPos.x >= spawnPosition.x &&
                    position.gridPos.y >= spawnPosition.y &&
                    position.gridPos.x < spawnPosition.x + type.spawnDimensions.x &&
                    position.gridPos.y < spawnPosition.y + type.spawnDimensions.y);

                if (!firstWave)
                    type.gameObject.SetActive(false);

                enemies.Add(Instantiate(
                    type.gameObject, spawnPosition + type.spawnOffset, Quaternion.identity).GetComponent<Enemy>());
                enemies[^1].enemyManager = this;

                type.gameObject.SetActive(true);

                if (!firstWave)
                    for (Vector2Int offset = Vector2Int.zero; offset.x < type.spawnDimensions.x; offset.x++)
                        for (offset.y = 0; offset.y < type.spawnDimensions.y; offset.y++)
                            Instantiate(smokePrefab, spawnPosition + offset + smokeOffset,
                                Quaternion.identity).GetComponent<EnableWithDelay>().Init(
                                    enemies[^1].gameObject, waveSpawnDelay);
            }
        }

        currentWave++;

        return true;
    }

    public bool CanSpawnAt(Enemy enemy, Vector2Int position, in List<EnemySpawnPosition> positions)
    {
        bool containsValid = false;
        for (Vector2Int offset = Vector2Int.zero; offset.x < enemy.spawnDimensions.x; offset.x++)
            for (offset.y = 0; offset.y < enemy.spawnDimensions.y; offset.y++)
            {
                int index = positions.FindIndex((compared) => compared.gridPos == position + offset);
                if ((index) == -1)
                    return false;
                if (!containsValid)
                    containsValid = positions[index].validSpawns.Contains(enemy);
            }
        return containsValid;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
            gameManager.inCombat = SpawnWave();
    }
}
