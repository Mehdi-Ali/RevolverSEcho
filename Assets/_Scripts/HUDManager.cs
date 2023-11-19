using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager HUD { get; private set; }

    public Image RightEchoBar;
    public Image LightEchoBar;

    private void Awake()
    {
        if (HUD == null)
        {
            HUD = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
