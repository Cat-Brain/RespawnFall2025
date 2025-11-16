using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelChunk
{
    public GameObject prefab;
    public Vector3 spawnOffset;
    public Vector2 dimensions;

    public GameObject Spawn(Vector2 offset, Transform parent = null)
    {
        return GameObject.Instantiate(prefab, spawnOffset + (Vector3)offset, Quaternion.identity, parent);
    }
}

public class GenerationManager : MonoBehaviour
{
    public GameManager gameManager;

    public float cameraSize;
    public List<LevelChunk> beginningChunks, middleChunks, endChunks;
    public int middleChunkCount;
    public float distanceBetweenRefreshes;

    public List<GameObject> currentChunks = new();

    public void Init()
    {
        float offset = -cameraSize;

        LevelChunk beginningChunk = beginningChunks[UnityEngine.Random.Range(0, beginningChunks.Count)];
        currentChunks.Add(beginningChunk.Spawn(Vector2.up * offset));
        offset += beginningChunk.dimensions.y;

        for (int i = 0; i < middleChunkCount; i++)
        {
            LevelChunk middleChunk = middleChunks[UnityEngine.Random.Range(0, middleChunks.Count)];
            currentChunks.Add(middleChunk.Spawn(Vector2.up * offset));
            offset += middleChunk.dimensions.y;
        }

        LevelChunk endChunk = endChunks[UnityEngine.Random.Range(0, endChunks.Count)];
        currentChunks.Add(endChunk.Spawn(Vector2.up * offset));
        offset += endChunk.dimensions.y;

        if (!gameManager.playerManager)
            gameManager.playerManager = FindFirstObjectByType<PlayerManager>();

        offset -= cameraSize;
        gameManager.playerManager.playerCamera.maxHeight = offset;
    }
}
