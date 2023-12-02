using System.Collections;
using System.Collections.Generic;
using System.Data;
using EasyButtons;
using UnityEngine;

public class DeformableTarget : MonoBehaviour
{

    [SerializeField] private GameObject _undamagedTransform;
    [SerializeField] private GameObject _damagedTransform;


    // Start is called before the first frame update
    void Start()
    {
        _undamagedTransform.SetActive(true);
        _damagedTransform.SetActive(false);
    }

    public void DamageTarget()
    {
        _undamagedTransform.SetActive(false);
        _damagedTransform.SetActive(true);
    }

}
