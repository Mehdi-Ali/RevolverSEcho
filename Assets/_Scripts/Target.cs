using System;
using Unity.Mathematics;
using UnityEngine;

public class DamageableTarget : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private DeformableTarget _deformableTarget;
    [SerializeField] private EnemyPhysics _enemyPhysics;
    public SplashType SplashType;
    private Rigidbody _rigidbody;
    private PoolSystem _PopupPool;
    private PoolSystem _VFXPool;


    void Start()
    {
        _PopupPool = PoolManager.PoolInst.DamagePopup;
        _VFXPool = PoolManager.PoolInst.EchoDamageVFX;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage, Vector3 contactPoint,  int id = -1, bool isEcho = false)
    {
        if (_deformableTarget != null)
            _deformableTarget.DamageTarget();
            
        if (this == null) return;

        _health -= damage;
        _health = Math.Max(_health, 0);

        if (!isEcho)
            DisplayNumbers(damage, contactPoint);
            
        else 
        {
            DisplayNumbers(damage, contactPoint, isEcho);
            StartEchoVFX(damage, contactPoint);

            if (_enemyPhysics != null)
                _enemyPhysics.TookDamage(damage);
        }

        if (_health <= 0)
            Die();
    }


    private void DisplayNumbers(float damage, Vector3 contactPoint, bool isEcho = false)
    {
        // we can make the damage types indexed as int and passed here and make a switch case
        Quaternion textInfoAsQuaternion;
        if (isEcho)
            textInfoAsQuaternion = new Quaternion(damage, 1f, 0f, 0f);

        else
            textInfoAsQuaternion = new Quaternion(damage, 0f, 0f, 0f);

        var damagePopup = _PopupPool.Get(transform.TransformPoint(contactPoint), textInfoAsQuaternion);
    }

    private void StartEchoVFX(float damage, Vector3 contactPoint)
    {
        // todo randomize rotation?
        var vfxInstance = _VFXPool.Get(transform.TransformPoint(contactPoint), transform.rotation);
        vfxInstance.transform.localScale = math.min((damage / 20f), 1f) * Vector3.one;
        _VFXPool.Return(vfxInstance, 2f);
    }

    public void ApplyForce(float magnitude, Vector3 direction = default)
    {
        if (direction == default)
        {
            direction = UnityEngine.Random.insideUnitSphere.normalized;
            direction.y = math.abs(direction.y);
        }
        _rigidbody.AddForce(magnitude * direction);
    }

    public void Die()
    {
        // TODO add a pool system for targets
        Destroy(this.gameObject);
    }
}

public enum SplashType
{
    Default,
    NoSplash,
    Metal,
    Wood,
    ElectronicEnemy,
    OrganicEnemy
}
