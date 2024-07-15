using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.LowLevel;

public class DamageableTarget : MonoBehaviour, IPool
{
    [Header("Enemy")]
    [SerializeField]
    private float _MaxHealth = 100f;
    private float _currentHealth;
    [SerializeField]
    private DeformableTarget _deformableTarget;
    [SerializeField]
    private BaseEnemy _baseEnemy;
    public bool isInvulnerable;
    [Space(16)]
    
    [Header("ParametersToSetOnSpawn")]
    [SerializeField] private bool _setPosition = true;
    [SerializeField] private bool _setRotation = false;
    [SerializeField] private bool _setScale = false;

    [Header("Span")]
    [SerializeField]
    private bool SpanOnSpawn;
    [SerializeField]
    private float _dissolveRate = 0.0125f;
    [SerializeField]
    private float _refreshRate = 0.025f;

    private List<Material> _materials;
    private MeshRenderer[] meshRenderers;
    [Space(16)]


    [Header("Splash")]
    public SplashType SplashType;
    [Space(16)]



    private Rigidbody _rigidbody;
    private PoolInstance _PopupPool;
    private PoolInstance _VFXPool;

    public PoolInstance poolInstance { get; set; }
    GameObject IPool.gameObject => this.gameObject;


    void Start()
    {
        _PopupPool = PoolManager.PoolInst.DamagePopup;
        _VFXPool = PoolManager.PoolInst.EchoDamageVFX;
        _rigidbody = GetComponent<Rigidbody>();
        _currentHealth = _MaxHealth;
    }

    public virtual void TakeDamage(float damage, Vector3 contactPoint,  int id = -1, bool isEcho = false)
    {
        if (_deformableTarget != null)
            _deformableTarget.DamageTarget();
            
        if (this == null || isInvulnerable)
            return;

        _currentHealth -= damage;
        _currentHealth = math.max(_currentHealth, 0);

        if (!isEcho)
            DisplayNumbers(damage, contactPoint);
            
        else 
        {
            DisplayNumbers(damage, contactPoint, isEcho);
            StartEchoVFX(damage, contactPoint);

            if (_baseEnemy != null)
                _baseEnemy.TookDamage(damage);
        }

        if (_currentHealth <= 0)
            Die();
    }


    private void DisplayNumbers(float damage, Vector3 contactPoint, bool isEcho = false)
    {
        if (damage == 0)
            return;
        
        // we can make the damage types indexed as int and passed here and make a switch case
        Quaternion textInfoAsQuaternion;
        if (isEcho)
            textInfoAsQuaternion = new Quaternion(damage, 1f, 0f, 0f);

        else
            textInfoAsQuaternion = new Quaternion(damage, 0f, 0f, 0f);

        var damagePopup = _PopupPool.SpawnFromPool(contactPoint, textInfoAsQuaternion);
    }

    private void StartEchoVFX(float damage, Vector3 contactPoint)
    {
        // todo randomize rotation?
        var vfxInstance = _VFXPool.SpawnFromPool(contactPoint, transform.rotation, this.transform);
        vfxInstance.gameObject.transform.localScale = math.min((damage / 20f), 1f) * Vector3.one;
        _VFXPool.ReturnToPool(vfxInstance, 2f);
    }

    public void ApplyForce(float magnitude, Vector3? contactPoint = null)
    {
        Vector3 direction;
        if (contactPoint == null)
        {
            direction = UnityEngine.Random.insideUnitSphere.normalized;
            direction.y = math.abs(direction.y);
        }
        else
            direction = (transform.position - (Vector3)contactPoint).normalized;

        _rigidbody.AddForce(magnitude * direction);

        var randomTorque = new Vector3( UnityEngine.Random.Range(-1f, 1f),
                                        UnityEngine.Random.Range(-1f, 1f),
                                        UnityEngine.Random.Range(-1f, 1f)).normalized;
                                        
        _rigidbody.AddTorque(randomTorque * magnitude);
    }

    public void Die()
    {
        PoolManager.PoolInst.Target.ReturnToPool(this);
    }

    public void Initialize(int id, Vector3 position, Quaternion rotation, Vector3 scale, float delay = 0f)
    {
        if (_setPosition)
            transform.position = position;
        if (_setRotation)
            transform.rotation = rotation;
        if (_setScale)
            transform.localScale = scale;
        
        _currentHealth = _MaxHealth;

        if (_deformableTarget != null)
            _deformableTarget.ResetTarget();

        if (SpanOnSpawn)
            StartCoroutine(SpawnVFX());
    }

    IEnumerator SpawnVFX()
    {
        if (_materials == null)
            GetChildrenMaterials();

        float randomOffset = UnityEngine.Random.Range(0, 10);
        foreach (var material in _materials)
            material.SetVector("_RandomOffset", new Vector2(randomOffset, randomOffset));

        float time = 1;
        while (time > 0)
        {
            time -= _dissolveRate;
            foreach (var material in _materials)
                material.SetFloat("_DissolveAmount", time);

            yield return new WaitForSeconds(_refreshRate);
        }
    }

    private void GetChildrenMaterials()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        _materials = new List<Material>();

        foreach (var meshRenderer in meshRenderers)
            _materials.AddRange(meshRenderer.materials);
    }

    public void ResetInst() {}
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
