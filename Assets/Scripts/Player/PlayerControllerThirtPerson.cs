using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerThirtPerson : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    public float groundedOffsetZ = 0.15f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    //Disparo
    [Header("Disparo")]
    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField]public List<CombatEnemies> enemies = new List<CombatEnemies>();
    
    [SerializeField] Transform _dirBullet;
    [SerializeField] Vector3 _minScale;
    [SerializeField] Vector3 _maxScale;
    bool _fire = false;
    [SerializeField] float _chargeTime;
    [SerializeField] float _recolding;
    [SerializeField] bool _canFire = true;
    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    EnergySystem _scriptEnergy;
#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private Animator _animator;
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;
    public GameObject _jostycs;
    private bool IsCurrentDeviceMouse {
        get {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }

    private void Awake() {
        // get a reference to our main camera
        if (_mainCamera == null) {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        _scriptEnergy = GetComponent<EnergySystem>();
    }
    private void Start() {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
#if UNITY_ANDROID
        _jostycs.SetActive(true);
        //_playerInput.enabled = false;
#else
_jostycs.SetActive(false);
_playerInput.enabled = true;
#endif
        GameManager.Instance.PlaySounds(1);
    }
    private void Update() {
        GroundedCheck();
        Move();
        Fallout();
        Fire();
    }

    private void LateUpdate() {
        CameraRotation();
    }
    private void GroundedCheck() {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z-groundedOffsetZ);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }
    private void CameraRotation() {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition) {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    private void Move() {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset) {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        } else {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_input.move != Vector2.zero) {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);


    }
    private void Fallout() {
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity) {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    private void OnDrawGizmosSelected() {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),GroundedRadius);
        
    }
    private void OnFootstep(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            if (FootstepAudioClips.Length > 0) {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
    private void OnLand(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    void Fire() {
        
        if (_input._fire) {//preciona la tecla de Disparo
            if (_scriptEnergy.canAttack()) {
                if (!_fire) {
                    if (!_canFire) return;
                    StartCharging();
                } else {
                    ChargeBullet();

                }
            } else {
                ShakeCamera();
                _fire = false;
            }
            
        }else if (_fire) { // si se suelta el boton
            ReleaseCharged();
            
        }
    }
    void StartCharging() {//precionamos la tecla de disparo 
       
        _animator.SetInteger("Fire", 0);
        _fire = true;
        _canFire = false;
        _chargeTime = 0;
        //Instanciamos la bala
        _bulletPrefab = Instantiate(_bullet, _dirBullet);
        _scriptEnergy.canReloading = false;

        _bulletPrefab.transform.localScale = _minScale;

    }
    void ChargeBullet() {
        _chargeTime += Time.deltaTime;
        
        float scaleProgress = Mathf.Clamp01(_chargeTime / _recolding);
        if (_bulletPrefab) {
            _bulletPrefab.transform.localScale = Vector3.Lerp(_minScale, _maxScale, scaleProgress);
        }
    }
    void ReleaseCharged() {
        _fire = false;
        
        //busca el enemigos mas cercano
        CombatEnemies closestEnemy = FindClosesEnemy();
        if (_bulletPrefab) {
            if(_chargeTime >= _recolding) { // disparo carga completado
                _animator.SetInteger("Fire", 2);
                _bulletPrefab.GetComponent<BulletController>().enabled = true;

                if (closestEnemy != null) {
                    _bulletPrefab.GetComponent<BulletController>().DetectTransform(closestEnemy.transform);
                } else {
                    _bulletPrefab.GetComponent<BulletController>().TransformDirection(transform.forward);

                }
                _scriptEnergy.ChancheEnergy();
                ShakeCamera();
                _scriptEnergy.canReloading = true;
            } else {
                Destroy(_bulletPrefab);
                _animator.SetInteger("Fire", 1);
                _canFire = true;
            }
        }
        _bulletPrefab = null;
    }
    public void ISCanFire() {
        _canFire = true;
    }
    void ShakeCamera() {
        LeanTween.moveLocal(CinemachineCameraTarget, CinemachineCameraTarget.transform.localPosition + (Vector3)Random.insideUnitCircle * 0.2f, 0.05f).setLoopPingPong(2).
            setOnComplete(() => CinemachineCameraTarget.transform.localPosition = new Vector3(0, 1.5f, 0));
    }
    CombatEnemies FindClosesEnemy() {
        CombatEnemies closestEnemy = null;
        float closestDistance = Mathf.Infinity;//comenzamos con una distancia muy grande
        enemies.RemoveAll(enemy => enemy == null);
        for (int i = 0; i < enemies.Count; i++) {
            if(enemies !=null){
                float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestEnemy = enemies[i];//guardamos la referencia al enemigo 
                }
            }
        }
        return closestEnemy;
    }
    public void AddEnemies(CombatEnemies enemiesCombat) {
        enemies.Add(enemiesCombat);
    }
    public void RemoveEnemies(CombatEnemies enemiesCombat) {
        enemies.Remove(enemiesCombat);
    }

}
