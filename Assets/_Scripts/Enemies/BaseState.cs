using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected BaseEnemy Enemy { get; private set; }

    public void EnterState(BaseEnemy enemy)
    {
        Enemy = enemy;
        OnEnterState();
    }

    public void EnterExit()
    {
        OnExitState();
        Enemy = null;
    }


    protected  abstract void OnEnterState();
    public abstract void OnUpdateState();
    protected  abstract void OnExitState();
}
