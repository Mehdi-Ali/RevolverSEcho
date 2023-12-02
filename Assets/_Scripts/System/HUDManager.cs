using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager HUD { get; private set; }

    public Image RightEchoBar;
    public Image LeftEchoBar;

    // private void Awake()
    // {
    //     if (HUD == null)
    //     {
    //         HUD = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void OnEnable()
    {
        EventSystem.Events.OnEchoChargeChanged += UpdateEchoCHarge;
    }

    private void UpdateEchoCHarge(string controllerName, float fillAmount)
    {
        if (controllerName == "Right Controller")
            RightEchoBar.fillAmount = fillAmount;

        else if (controllerName == "Left Controller")
            LeftEchoBar.fillAmount = fillAmount;
    }

    void OnDisable()
    {
        EventSystem.Events.OnEchoChargeChanged -= UpdateEchoCHarge;
    }
}
