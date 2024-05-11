using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public bool autostart;

    [SerializeField]
    private Transform[] targetsSpawningPoints;
    private int targetCurrentIndex = 0;
    [SerializeField]
    private Transform[] cansSpawningPoints;
    private int canCurrentIndex = 0;


    void Start()
    {
        if (autostart)
            StartSpawning();
    }

//    [Button]
    void StartSpawning()
    {
        StartCoroutine(SpawnTargetsCR());
        StartCoroutine(SpawnCansCR());
    }

    private IEnumerator SpawnTargetsCR()
    {
        if (targetCurrentIndex >= targetsSpawningPoints.Length)
            yield break;

        Transform spawnPoint = targetsSpawningPoints[targetCurrentIndex++];
        PoolManager.PoolInst.Target.SpawnFromPool(spawnPoint);
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnTargetsCR());
    }

    private IEnumerator SpawnCansCR()
    {
        if (canCurrentIndex >= cansSpawningPoints.Length)
            yield break;

        Transform spawnPoint = cansSpawningPoints[canCurrentIndex++];
        PoolManager.PoolInst.TargetCan.SpawnFromPool(spawnPoint);
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnCansCR());
    }

}
