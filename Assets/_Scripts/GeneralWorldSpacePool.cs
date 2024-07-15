using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class GeneralWorldSpacePool : MonoBehaviour, IPool
{
    [Header("ParametersToSetOnSpawn")]
    [SerializeField] private bool _setPosition = true;
    [SerializeField] private bool _setRotation = true;
    [SerializeField] private bool _setScale = false;
    [Space]
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;

    public PoolInstance poolInstance { get; set; }
    GameObject IPool.gameObject => this.gameObject;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void Initialize(int _, Vector3 position, Quaternion rotation, Vector3 scale, float delay = 0f)
    {
        if (_setPosition)
            transform.position = position;
        if (_setRotation)
            transform.rotation = rotation;
        if (_setScale)
            transform.localScale = scale;

        if (_audioClip != null)
            _audioSource.PlayOneShot(_audioClip);
        if (_particleSystem != null)
            _particleSystem?.Play(true);
    }

    public void ResetInst() { }
}
