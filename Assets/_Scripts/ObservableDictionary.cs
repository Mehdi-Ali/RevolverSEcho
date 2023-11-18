using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class ObservableDictionary : MonoBehaviour
{
#if UNITY_EDITOR
    public Dictionary<int, float> PerformanceDictionary;

    [SerializeField] private List<int> keys = new();
    [SerializeField] private List<float> values = new();

    [Button]
    private void Update()
    {
        keys.Clear();
        values.Clear();
        foreach (var elem in GetComponent<RecoilEvaluation>().PerformanceDictionary)
        {
            keys.Add(elem.Key);
            values.Add(elem.Value);
        }
    }
#endif
}
