using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PoolInstance : MonoBehaviour
{
    public GameObject IPoolPrefab;
    public int normalSize;
    public bool DynamicParent = false;

    private Queue<IPool> pool;
    private int _nextID = 0;
    public int NextID
    {
        get { return _nextID++; }
    }


    void Start()
    {
        pool = new Queue<IPool>();
        for (int i = 0; i < normalSize; i++)
        {
            InstantiateObject();
        }
    }

    private IPool InstantiateObject(bool active = false)
    {
        GameObject instance = Instantiate(IPoolPrefab, transform);
        instance.SetActive(active);

        IPool iPool = instance.GetComponent<IPool>();
        if (iPool != null)
            pool.Enqueue(iPool);

        return iPool;
    }

    public IPool SpawnFromPool(Transform spawnPointTransform, Transform parent = null)
    {
        if (spawnPointTransform == null)
            return null;

        return SpawnFromPool(spawnPointTransform.position, spawnPointTransform.rotation, spawnPointTransform.localScale, out _, parent);
    }
    public IPool SpawnFromPool(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        Vector3 scale = Vector3.zero;
        return SpawnFromPool(position, rotation, scale, out _, parent);
    }
    public IPool SpawnFromPool(Vector3 position, Quaternion rotation, out int id, Transform parent = null)
    {
        Vector3 scale = Vector3.zero;
        return SpawnFromPool(position, rotation, scale, out id, parent);
    }
    public IPool SpawnFromPool(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
    {
        return SpawnFromPool(position, rotation, scale, out _, parent);
    }
    public IPool SpawnFromPool(Vector3 position, Quaternion rotation, Vector3 scale, out int id, Transform parent = null)
    {
        IPool instance;
        if (pool != null && pool.Count > 0)
        {
            instance = pool.Dequeue();
            instance.gameObject.SetActive(true);
        }
        else
        {
            instance = InstantiateObject(true);
        }

        id = NextID;
        if (instance != null)
        {
            instance.Initialize(id, position, rotation, scale);
            instance.poolInstance = this;
        }

        if (parent != null && DynamicParent)
            instance.gameObject.transform.SetParent(parent);

        return instance;
    }


    public void ReturnToPool(IPool instance, float delay = 0f)
    {
        StartCoroutine(DelayedReturn(instance, delay));
    }

    public IEnumerator DelayedReturn(IPool instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);

        if (instance != null)
        {
            instance.gameObject.transform.SetParent(this.transform);
            instance.ResetInst();
        }
    }

}

public interface IPool
{
    public PoolInstance poolInstance { get; set;}
    public GameObject gameObject { get;}

    public void Initialize(int id, Vector3 position, Quaternion rotation, Vector3 scale, float delay = 0f);
    public void ResetInst();
}