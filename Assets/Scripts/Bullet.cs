using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletSplash;


    private Stopwatch shootingStopwatch;


    private void Start()
    {
        shootingStopwatch = new Stopwatch();
        shootingStopwatch.Start();
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;

        CreateSplash(collision.GetContact(0).point);
        ShootingEnded();
        Destroy(gameObject);
    }


    private void CreateSplash(Vector3 splashPosition)
    {

        var bulletSplash = (GameObject)Instantiate(
            _bulletSplash,
            splashPosition,
            transform.rotation);

        // Destroy the bullet after 2 seconds
        Destroy(bulletSplash, 15.0f);
    }


    private void ShootingEnded()
    {
        shootingStopwatch.Stop();
        UnityEngine.Debug.Log($"shooting duration: {shootingStopwatch.Elapsed.TotalSeconds} seconds");
        shootingStopwatch.Reset();
    }
}
