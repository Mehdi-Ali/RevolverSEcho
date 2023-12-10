using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected BaseEnemy Enemy {get; private set; }

    void Start()
    {
        Enemy = GetComponentInParent<BaseEnemy>();
    }

    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}
