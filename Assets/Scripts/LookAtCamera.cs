using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Mode mode;
    
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }
    
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                // 예전에는 Camera.main이 캐시되지 않아서 배드 퍼포먼스여서 사용하지 않았으나 지금은 디폴트로 캐시되어서 사용
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
