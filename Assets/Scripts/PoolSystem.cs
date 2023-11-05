using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public GameObject IPoolPrefab;
    public int normalSize;
    private Queue<GameObject> pool;


    void Start()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < normalSize; i++)
        {
            GameObject instance = Instantiate(IPoolPrefab);
            instance.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject instance;
        if (pool.Count > 0)
        {
            instance = pool.Dequeue();
            instance.SetActive(true);
        }
        else
            instance = Instantiate(IPoolPrefab, transform);

        if (instance.TryGetComponent<IPool>(out var iPool))
            iPool.Initialize(position, rotation);
        
        return instance;
    }

    public IEnumerator Return(GameObject instance, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
        pool.Enqueue(instance);

        if (instance.TryGetComponent<IPool>(out var iPool))
            iPool.ResetInst();
    }

}

public interface IPool
{
    public void Initialize(Vector3 position, Quaternion rotation);
    public void ResetInst();
}