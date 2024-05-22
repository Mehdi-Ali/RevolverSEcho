using System.Threading;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using Unity.XR.CoreUtils;
using UnityEngine;

public class SequencerSpawner : MonoBehaviour
{
    public bool autostart;

    public SpawningEntity[] SpawningEntities;



    void Start()
    {
        if (autostart)
            StartCoroutine(SpawnCoroutine());
    }

    [Button]
    private IEnumerator SpawnCoroutine()
    {
        for (int i = 0; i < SpawningEntities.Length; i++)
        {
            SpawningEntity spawningEntity = SpawningEntities[i];
            StartCoroutine(SpawnEntityElements(spawningEntity));
        }
        yield return null;
    }

    [Button]
    public void SpawnEntity(int entityIndex, int start = -1, int end = -1)
    {
        int? nullableStart = (start == -1) ? null : start;
        int? nullableEnd = (end == -1) ? null : end;

        StartCoroutine(SpawnEntityElements(SpawningEntities[entityIndex], nullableStart, nullableEnd));
    }

    private IEnumerator SpawnEntityElements(SpawningEntity spawningEntity, int? nullableStart = null, int? nullableEnd = null)
    {
        PoolInstance pool = spawningEntity.PoolInstance;
        if (pool == null)
            yield break;
        
        Transform[] spawningPoints = spawningEntity.SpawningPoints;
        if (spawningPoints == null || spawningPoints.Length == 0)
            yield break;

        int start = (nullableStart == null || nullableStart >= spawningPoints.Length) ? spawningEntity.currentIndex :  (int)nullableStart;
        int end = (nullableEnd == null || nullableStart >= spawningPoints.Length) ? spawningPoints.Length : (int)nullableEnd;

        for (int j = start++; j < end; j++)
        {
            Transform spawningPoint = spawningPoints[j];
            spawningEntity.PoolInstance.SpawnFromPool(spawningPoint);
            yield return new WaitForSeconds(1);
        }
    }

    [Button]
    void SpawnNextObject(int entityIndex)
    {
        SpawningEntity spawningEntity = SpawningEntities[entityIndex];
        PoolInstance pool = spawningEntity.PoolInstance;
        if (pool != null)
            pool.SpawnFromPool(spawningEntity.SpawningPoints[spawningEntity.currentIndex++]);

    }

}

[System.Serializable]
public struct SpawningEntity
{
    public PoolInstance PoolInstance;
    public Transform[] SpawningPoints;
    public int currentIndex;
}
