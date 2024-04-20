using UnityEngine;

public class DisableInBuild : MonoBehaviour
{
    public bool disableInBuild = true;

    private void Awake()
    {
        if (!Application.isEditor)
        {
            gameObject.SetActive(disableInBuild);
        }
    }
}
