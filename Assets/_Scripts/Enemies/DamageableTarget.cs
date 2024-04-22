using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class DamageableTarget : MonoBehaviour, IPool
{
    [SerializeField] private float _MaxHealth = 100f;
    [SerializeField] private DeformableTarget _deformableTarget;
    [SerializeField] private BaseEnemy _baseEnemy;
    public SplashType SplashType;
    private float _currentHealth;
    private Rigidbody _rigidbody;
    private PoolInstance _PopupPool;
    private PoolInstance _VFXPool;

    public bool isInvulnerable;


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

        var damagePopup = _PopupPool.Get(contactPoint, textInfoAsQuaternion);
    }

    private void StartEchoVFX(float damage, Vector3 contactPoint)
    {
        // todo randomize rotation?
        var vfxInstance = _VFXPool.Get(contactPoint, transform.rotation);
        vfxInstance.transform.localScale = math.min((damage / 20f), 1f) * Vector3.one;
        _VFXPool.Return(vfxInstance, 2f);
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
        PoolManager.PoolInst.DamageableTarget.Return(gameObject);
    }

    public void Initialize(int id, Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        _currentHealth = _MaxHealth;
        _deformableTarget.ResetTarget();
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
