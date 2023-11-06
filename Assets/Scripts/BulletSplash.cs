using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplash : MonoBehaviour, IPool
{
    [SerializeField] private ParticleSystem _bulletSplashVFX;
    [SerializeField] private AudioClip _splashSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void Initialize(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        audioSource.PlayOneShot(_splashSound);
        _bulletSplashVFX.Play(true);
    }

    public void ResetInst() {}
}
