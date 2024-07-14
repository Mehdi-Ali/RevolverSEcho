using System.Collections;
using System;
using EasyButtons;
using UnityEngine;

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
    public float spawnDelay = 0.5f;

    public SpawningEntity[] SpawningEntities;


    void Start()
    {
        if (autostart)
            StartCoroutine(SpawnAllCoroutine());
    }

    [Button]
    private void SpawnAll()
    {
        StartCoroutine(SpawnAllCoroutine());
    }

    private IEnumerator SpawnAllCoroutine()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (var spawningEntity in SpawningEntities)
        {
            PoolInstance pool = spawningEntity.PoolInstance;
            if (pool == null)
                continue;

            Transform[] spawningPoints = spawningEntity.SpawningPoints;
            if (spawningPoints == null || spawningPoints.Length == 0)
                continue;

            foreach (var spawnTrans in spawningEntity.SpawningPoints)
            {
                pool.SpawnFromPool(spawnTrans);
                spawningEntity.currentIndex++;
                yield return new WaitForSeconds(spawnDelay); // Add a delay here
            }
        }


        // [Button]
        // private void Spawn()
        // {
        //     StartCoroutine(SpawnCoroutine());
        // }

        // private IEnumerator SpawnCoroutine()
        // {
        //     for (int i = 0; i < SpawningEntities.Length; i++)
        //     {
        //         SpawningEntity spawningEntity = SpawningEntities[i];
        //         StartCoroutine(SpawnEntityElements(spawningEntity));
        //     }
        //     yield return null;
        // }
        //
        // [Button]
        // public void SpawnEntity(int elementIndex, int start = -1, int end = -1)
        // {
        //     int? nullableStart = (start == -1) ? null : start;
        //     int? nullableEnd = (end == -1) ? null : end;
        //
        //     StartCoroutine(SpawnEntityElements(SpawningEntities[elementIndex], nullableStart, nullableEnd));
        // }
        //
        // private IEnumerator SpawnEntityElements(SpawningEntity spawningEntity, int? nullableStart = null, int? nullableEnd = null)
        // {
        //     PoolInstance pool = spawningEntity.PoolInstance;
        //     if (pool == null)
        //         yield break;
        //     
        //     Transform[] spawningPoints = spawningEntity.SpawningPoints;
        //     if (spawningPoints == null || spawningPoints.Length == 0)
        //         yield break;
        //
        //     int start = (nullableStart == null || nullableStart >= spawningPoints.Length) ? 
        //         spawningEntity.currentIndex : (int)nullableStart;
        //     
        //     int end = (nullableEnd == null || nullableStart >= spawningPoints.Length) ?
        //         spawningPoints.Length : (int)nullableEnd;
        //
        //     for (int i = start; i < end; i++)
        //     {
        //         Transform spawningPoint = spawningPoints[i];
        //     
        //         yield return new WaitForSeconds(1);
        //         var obj = spawningEntity.PoolInstance.SpawnFromPool(spawningPoint);
        //         spawningEntity.currentIndex ++;
        //     }
        // }
        //
        // [Button]
        // void SpawnNextObject(int entityIndex)
        // {
        //     SpawningEntity spawningEntity = SpawningEntities[entityIndex];
        //     PoolInstance pool = spawningEntity.PoolInstance;
        //     if (pool != null)
        //         pool.SpawnFromPool(spawningEntity.SpawningPoints[spawningEntity.currentIndex++]);
        //
        //     // if we finsih with a element we should go to the next one 
        //
        // }

    }
}


