using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager PoolInst { get; private set; }

    public PoolInstance Bullet;
    public PoolInstance BulletSplash;
    public PoolInstance DamagePopup;
    public PoolInstance EchoDamageVFX;
    public PoolInstance TargetCan;
    public PoolInstance TargetDrone;
    public PoolInstance Target;

    private void Awake()
    {
        if (PoolInst == null)
        {
            PoolInst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
