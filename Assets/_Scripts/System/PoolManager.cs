using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager PoolInst { get; private set; }

    public PoolSystem Bullet;
    public PoolSystem BulletSplash;
    public PoolSystem DamagePopup;
    public PoolSystem EchoDamageVFX;
    public PoolSystem TargetCan;
    public PoolSystem TargetDrone;

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
