using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField]
    private bool _loop;
    [SerializeField]
    private float _dissolveRate = 0.0125f;
    [SerializeField]
    private float _refreshRate = 0.025f;

    private List<Material> _materials;
    private MeshRenderer[] meshRenderers;


    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        _materials = new List<Material>();

        foreach (var meshRenderer in meshRenderers)
            _materials.AddRange(meshRenderer.materials);

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        Vector2 randomOffset = new Vector2(Random.Range(0, 10), Random.Range(0, 10));
        foreach (var material in _materials)
            material.SetVector("_RandomOffset", randomOffset);

        float time = 1;
        while (time > 0)
        {
            time -= _dissolveRate;
            foreach (var material in _materials)
                material.SetFloat("_DissolveAmount", time);

            yield return new WaitForSeconds(_refreshRate);
        }

        if (!_loop)
            yield break;

        yield return new WaitForSeconds(2);
        StartCoroutine(Spawn());
    }

}
