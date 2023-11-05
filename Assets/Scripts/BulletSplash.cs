using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplash : MonoBehaviour, IPool
{
    [SerializeField] private ParticleSystem _muzzleFlashVFX;
    [SerializeField] private AudioClip _splashSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void Initialize(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        audioSource.PlayOneShot(_splashSound);
        _muzzleFlashVFX.Play(true);
    }

    public void ResetInst() {}
}
