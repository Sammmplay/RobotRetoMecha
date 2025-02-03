using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRobot : MonoBehaviour
{
    PlayerInput _input;

    [Header("Movement")]
    Rigidbody _rb;
    GameObject _mainCamera;
    public float _velocity = 2;
    [SerializeField] float _smoothRotation;
    float _targetRotation;
    float _rotationVelocity;
    [SerializeField] Vector3 _inputValue;
    private void Awake() {
        if(_mainCamera == null) {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    private void Start() {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Movement();
    }

    void OnMove(InputValue value) {
        _inputValue = value.Get<Vector2>();
    }
    void Movement() {
        Vector3 direction = new Vector3(_inputValue.x, 0,_inputValue.y) * _velocity * Time.fixedDeltaTime * 10;
        _rb.velocity = direction;
    }

}
