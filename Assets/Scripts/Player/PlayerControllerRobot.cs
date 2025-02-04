using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRobot : MonoBehaviour
{
    PlayerInput _input;
    public Transform cameraPivot;
    [Header("Movement")]
    Rigidbody _rb;
    GameObject _mainCamera;
    public float _velocity = 2;
    [SerializeField] float _rotationSpped = 10f;
    [SerializeField] float _rotationVelocity;
    [SerializeField] float RotationSmoothTime;
    [SerializeField] Vector3 _inputValue;
    [SerializeField] float _targetRotation;
    [Header("CheckGround")]
    [SerializeField] Transform _pivoctCheckSphere;
    [SerializeField] float _raycastDistance = 0.2f;
    [SerializeField] float _sphereCheck = 0.3f;
    [SerializeField]bool _isGround = false;
    private void Awake() {
        if(_mainCamera == null) {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    private void Start() {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        cameraPivot = FindObjectOfType<CameraController>().cameraPivot;
    }
    private void Update() {
        CheckGround();
    }
    private void FixedUpdate() {
        
        Movement();
    }

    void OnMove(InputValue value) {
        _inputValue = value.Get<Vector2>();
    }
    void Movement() {
        if (cameraPivot == null) return;
        if (!_isGround) return;
        Vector3 forward = cameraPivot.forward;
        Vector3 rigth = cameraPivot.right;
        forward.y = 0;
        rigth.y = 0;
        forward.Normalize();
        rigth.Normalize();

        Vector3 direction = (rigth * _inputValue.x + forward * _inputValue.y) * _velocity * Time.fixedDeltaTime * 10;
        _rb.velocity = direction;
        //si hay moviiento, giramos al personaje en esa direccion
        if (_inputValue != Vector3.zero) {
            _targetRotation = Mathf.Atan2(_inputValue.x, _inputValue.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

    void CheckGround() {
        _isGround = Physics.SphereCast(_pivoctCheckSphere.position, _sphereCheck, Vector3.down, out RaycastHit hit, _raycastDistance);   
     }
    private void OnDrawGizmos() {
        Gizmos.color = _isGround ? Color.green : Color.red;
        Gizmos.DrawLine(_pivoctCheckSphere.position, _pivoctCheckSphere.position + Vector3.down * _raycastDistance);
        Gizmos.DrawWireSphere(_pivoctCheckSphere.position + Vector3.down * _raycastDistance,_sphereCheck);
    }
}
