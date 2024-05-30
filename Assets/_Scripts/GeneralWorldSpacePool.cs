using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GeneralWorldSpacePool : MonoBehaviour, IPool
{
    [Header("ParametersToSet")]
    [SerializeField] private bool _position;
    [SerializeField] private bool _rotation;
    [SerializeField] private bool _scale;
    [Space]
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void Initialize(int _, Vector3 position, Quaternion rotation, Vector3 scale, float delay = 0f)
    {
        if (_position)
            transform.position = position;
        if (_rotation)
            transform.rotation = rotation;
        if (_scale)
            transform.localScale = scale;

        if (_audioClip != null)
            _audioSource.PlayOneShot(_audioClip);
        if (_particleSystem != null)
            _particleSystem?.Play(true);
    }

    public void ResetInst() {}
}
