using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRobot : MonoBehaviour
{
    public CinemachineFreeLook _freeLookCamera;
    public float _minAngle = -90f;// limite izquierdo
    public float _maxAngle = 90f; // limite derecho

    private void Update() {
        if(_freeLookCamera != null) {
            float currentAngle = _freeLookCamera.m_XAxis.Value;
            _freeLookCamera.m_XAxis.Value = Mathf.Clamp(currentAngle, _minAngle, _maxAngle);
        }
    }
}
