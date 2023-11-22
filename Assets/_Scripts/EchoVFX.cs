using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EchoManager))]
public class EchoVFX : MonoBehaviour
{
    private EchoManager _Manager;

    void Start()
    {
        _Manager = GetComponent<EchoManager>();
    }
}
