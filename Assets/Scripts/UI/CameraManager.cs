using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] CinemachineVirtualCamera _vcam;
    private void Awake() {
        instance = this;
    }
    private void Start() {
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }
    public void FollowCam(Transform folow ) {
        _vcam.Follow = folow;
        _vcam.LookAt = folow;
    }


}
