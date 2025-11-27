using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct EnemyTierData
{
    public int tier, typeCount, totalQuantity;
}

[Serializable]
public struct LevelData
{
    public List<EnemyTierData> enemyTierData;
}

public class EnemyManager : MonoBehaviour
{
    public GameManager gameManager;
    public GenerationManager generationManager;

    public List<LevelData> levelData;

    public List<Enemy> enemies;

    public void LoadEnemiesInLevel(GameObject levelParent)
    {
        List<EnemySpawnPosition> spawnPositions =
            levelParent.GetComponentsInChildren<EnemySpawnPosition>().ToList();

        if (spawnPositions.Count <= 0)
        {
            Debug.LogWarning("Missing Enemy Spawn Postions!!!");
            return;
        }

        IEnumerable<Enemy> validEnemies = Enumerable.Empty<Enemy>();
        foreach (EnemySpawnPosition position in spawnPositions)
            validEnemies = validEnemies.Union(position.validSpawns);

        foreach (EnemyTierData tierData in levelData[gameManager.level].enemyTierData)
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
                    spawnPositions.Where((position) => CanSpawnAt(type, position.gridPos, spawnPositions)).ToList();
                Vector2Int spawnPosition = validSpawns[UnityEngine.Random.Range(0, validSpawns.Count)].gridPos;

                spawnPositions.RemoveAll((position) =>
                    position.gridPos.x >= spawnPosition.x &&
                    position.gridPos.y >= spawnPosition.y &&
                    position.gridPos.x < spawnPosition.x + type.spawnDimensions.x &&
                    position.gridPos.y < spawnPosition.y + type.spawnDimensions.y);

                enemies.Add(Instantiate(
                    type.gameObject, spawnPosition + type.spawnOffset, Quaternion.identity).GetComponent<Enemy>());
                enemies[enemies.Count - 1].enemyManager = this;
            }
        }

        gameManager.inCombat = enemies.Count > 0;
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
        gameManager.inCombat = enemies.Count > 0;
    }
}
