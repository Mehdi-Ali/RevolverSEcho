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
    public float spawningDelay = 0.1f;

    public SpawningEntity[] SpawningEntities;

    private int currentEntityIndex = 0;
    private int currentPointIndex = 0;

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
            GameObject spawnedObject = pool.SpawnFromPool(spawnTrans);
            if (spawnedObject != null)
            {
                spawnedObject.transform.position = spawnTrans.position;
                spawnedObject.transform.rotation = spawnTrans.rotation;
            }
            spawningEntity.currentIndex++;
        }

        currentPointIndex++;

        Invoke(nameof(SpawnNext), spawningDelay);
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


