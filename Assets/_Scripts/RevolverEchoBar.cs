using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevolverEchoBar : MonoBehaviour
{
    public Image EchoBar;
    [SerializeField] private string _controllerName = "Right Controller / Left Controller";

    void OnEnable()
    {
        EventSystem.Events.OnEchoChargeChanged += UpdateEchoCHarge;
    }

    private void UpdateEchoCHarge(string controllerName, float fillAmount)
    {
        if (controllerName == _controllerName)
            EchoBar.fillAmount = fillAmount / 2 ;
    }

    void OnDisable()
    {
        EventSystem.Events.OnEchoChargeChanged -= UpdateEchoCHarge;
    }
}