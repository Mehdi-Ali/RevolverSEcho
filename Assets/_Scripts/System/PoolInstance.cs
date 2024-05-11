using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PoolInstance : MonoBehaviour
{
    public GameObject IPoolPrefab;
    public int normalSize;
    public bool DynamicParent = false;

    private Queue<GameObject> pool;
    private int _nextID = 0;
    public  int NextID
    {
        get { return _nextID++; }
    }


    void Start()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < normalSize; i++)
        {
            GameObject instance = Instantiate(IPoolPrefab, transform);
            instance.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return SpawnFromPool(position, rotation, out _, parent);
    }
    public GameObject SpawnFromPool(Transform spawnPointTransform, Transform parent = null)
    {
        if (spawnPointTransform == null)
            return null;

        return SpawnFromPool(spawnPointTransform.position, spawnPointTransform.rotation, out _, parent);
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation , out int id, Transform parent = null)
    {
        GameObject instance;
        if (pool != null && pool.Count > 0)
        {
            instance = pool.Dequeue();
            instance.SetActive(true);
        }
        else
            instance = Instantiate(IPoolPrefab, transform);

        id = NextID;
        if (instance.TryGetComponent<IPool>(out var iPool))
            iPool.Initialize(id, position, rotation);

        if (parent != null && DynamicParent)
            instance.transform.SetParent(parent);
        
        return instance;
    }


    public void ReturnTomPool(GameObject instance, float delay = 0f)
    {
        StartCoroutine(DelayedReturn(instance, delay));
    }
    
    public IEnumerator DelayedReturn(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
        pool.Enqueue(instance);

        if (instance.TryGetComponent<IPool>(out var iPool))
        {
            instance.transform.SetParent(this.transform);
            iPool.ResetInst();
        }
    }

}

public interface IPool
{
    public void Initialize(int id, Vector3 position, Quaternion rotation);
    public void ResetInst();
}