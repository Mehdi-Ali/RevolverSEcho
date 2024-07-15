using System.Collections;
using System;
using EasyButtons;
using UnityEngine;
using System.Collections.Generic;

public class SequenceSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawningEntity
    {
        public PoolInstance PoolInstance;
        public Transform[] SpawningPoints;
        public int currentIndex;
    }

    public bool autostart;
    public float startDelay = 3f;
    public float spawningDelay = 0.1f;

    public SpawningEntity[] SpawningEntities;

    private int currentEntityIndex = 0;
    private int currentPointIndex = 0;
    private List<IPool> spawnedObjects = new List<IPool>();

    void Start()
    {
        if (autostart)
            Invoke(nameof(SpawnAll), startDelay);
    }

    [Button]
    private void SpawnAll()
    {
        currentEntityIndex = 0;
        currentPointIndex = 0;
        SpawnNext();
    }

    private void SpawnNext()
    {
        if (currentEntityIndex >= SpawningEntities.Length)
        {
            Debug.Log("Spawning completed");
            return;
        }

        var spawningEntity = SpawningEntities[currentEntityIndex];
        PoolInstance pool = spawningEntity.PoolInstance;

        if (pool == null || spawningEntity.SpawningPoints == null || spawningEntity.SpawningPoints.Length == 0)
        {
            currentEntityIndex++;
            currentPointIndex = 0;
            SpawnNext();
            return;
        }

        if (currentPointIndex >= spawningEntity.SpawningPoints.Length)
        {
            currentEntityIndex++;
            currentPointIndex = 0;
            SpawnNext();
            return;
        }

        Transform spawnTrans = spawningEntity.SpawningPoints[currentPointIndex];

        if (spawnTrans != null)
        {
            IPool spawnedObject = pool.SpawnFromPool(spawnTrans);
            if (spawnedObject != null)
            {
                spawnedObject.gameObject.transform.position = spawnTrans.position;
                spawnedObject.gameObject.transform.rotation = spawnTrans.rotation;
                spawnedObjects.Add(spawnedObject);
            }
            spawningEntity.currentIndex++;
        }

        currentPointIndex++;

        Invoke(nameof(SpawnNext), spawningDelay);
    }

    [Button]
    public void DespawnAll()
    {
        foreach (IPool spawnedObject in spawnedObjects)
        {
            if (spawnedObject != null)
            {
                PoolInstance pool = spawnedObject.poolInstance;
                pool.ReturnToPool(spawnedObject);
            }
        }

        spawnedObjects.Clear();
    }

    private PoolInstance FindPoolForObject(GameObject obj)
    {
        foreach (var entity in SpawningEntities)
        {
            if (entity.PoolInstance != null && entity.PoolInstance)
            {
                return entity.PoolInstance;
            }
        }
        return null;
    }
}