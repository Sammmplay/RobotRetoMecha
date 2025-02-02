using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SD
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;
        public Camera _Camera;
        [SerializeField] CinemachineVirtualCamera _CameraVirtualCamera;
        [SerializeField] CinemachineConfiner2D _confiner2D;
        [SerializeField] GameObject _player;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Destroy(gameObject);
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            _Camera = camera.GetComponent<Camera>();
            if(_CameraVirtualCamera == null)
            {
                _CameraVirtualCamera =GetComponentInChildren<CinemachineVirtualCamera>();
            }
            
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
           
        }
        private void Update()
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            VirtualCamera();
        }
        private void VirtualCamera()
        {

            if (_player != null)
            {
                _CameraVirtualCamera.Follow = _player.GetComponent<Transform>();
            }
            if (SceneManager.GetActiveScene().buildIndex!=0)
            {
                GameObject _confinerCollider = GameObject.FindGameObjectWithTag("ConfinerCollider");
                _confiner2D.m_BoundingShape2D = _confinerCollider.GetComponent<Collider2D>();
            }
            
        }
    }
}

