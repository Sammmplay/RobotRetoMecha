using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] CinemachineVirtualCamera _cinemachineVCamera;
    public Transform cameraPivot; //punto vacio como referencia de la camera;
    public float sensitivy = 1.0f;
    public float minXRotation = -30f;
    public float maxXRotation = 60f;
    public float rotationSpeed = 200f;

    [SerializeField] Vector2 _lookImput;
    float currentYaw; //rotacion horizontal de la camara al rededor del personaje 
    float currentPitch; // rotacion vertical de la camara, limitada para evitar movimientos bruscos

    private void Start() {
        _cinemachineVCamera = FindObjectOfType<CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentYaw = transform.eulerAngles.y;
    }
    private void Update() {
        if(_cinemachineVCamera != null && cameraPivot != null) {
            RotationCamera();
        }
    }
    public void OnLook(InputValue value) {
        _lookImput = value.Get<Vector2>();
    }
    void RotationCamera() {
        float deltaX = _lookImput.x * sensitivy * Time.deltaTime * rotationSpeed;
        float deltaY = _lookImput.y * sensitivy * Time.deltaTime * rotationSpeed;
        currentYaw += deltaX;
        currentPitch -= deltaY;
        currentPitch = Mathf.Clamp(currentPitch, minXRotation, maxXRotation);
        //aplicamos la rotacion al pivit en lugar del jugador directamente
        cameraPivot.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
    }
}
