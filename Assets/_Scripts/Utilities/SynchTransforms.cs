using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchTransforms : MonoBehaviour
{
    [SerializeField]
    private Transform transformToCopy;
    [SerializeField]
    private Transform transformToSynch;
    
    [Header("Position")]
    [SerializeField]
    private bool xPosition = true;
    [SerializeField]
    private bool yPosition = true;
    [SerializeField]
    private bool zPosition = true;
    [Header("Rotation")]
    [SerializeField]
    private bool xRotation = true;
    [SerializeField]
    private bool yRotation = true;
    [SerializeField]
    private bool zRotation = true;
    [SerializeField]
    private bool wRotation = true;


    private void Update()
    {
        if (transformToCopy == null || transformToSynch == null)
            return;

        Vector3 synchedPos = new(transformToSynch.localPosition.x, transformToSynch.localPosition.y, transformToSynch.localPosition.z);
        Quaternion synchedRot = new(transformToSynch.localRotation.x, transformToSynch.localRotation.y, transformToSynch.localRotation.z, transformToSynch.localRotation.w);
        
        if (xPosition)
            synchedPos.x = transformToCopy.position.x;
        if (yPosition)
            synchedPos.y = transformToCopy.position.y; ;
        if (zPosition)
            synchedPos.z = transformToCopy.position.z; ;

        if (xRotation)
            synchedRot.x = transformToCopy.rotation.x;
        if (yRotation)
            synchedRot.y = transformToCopy.rotation.y; ;
        if (zRotation)
            synchedRot.z = transformToCopy.rotation.z; ;
        if (wRotation)
            synchedRot.z = transformToCopy.rotation.w; ;

        transformToSynch.localPosition = synchedPos;
        transformToSynch.localRotation = synchedRot;
    }
}