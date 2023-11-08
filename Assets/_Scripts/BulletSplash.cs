using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplash : MonoBehaviour, IPool
{
    [SerializeField] private ParticleSystem _bulletSplashVFX;
    [SerializeField] private AudioClip _splashSound;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void Initialize(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        if (_audioSource != null) 
            _audioSource.PlayOneShot(_splashSound);
        if (_bulletSplashVFX != null) 
            _bulletSplashVFX.Play(true);
    }

    public void ResetInst() {}
}
