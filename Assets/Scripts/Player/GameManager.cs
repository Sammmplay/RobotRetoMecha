using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Transform root;
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem _effectStartgame;
    public bool _starGame = false;
    Button _empezar;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else Destroy(gameObject);
    }
    public void Empezar() {
        _effectStartgame = GameObject.Find("InstantiatePlayer").GetComponent<ParticleSystem>();
        StartCoroutine(StartGame());
        
    }
    IEnumerator StartGame() {
        _effectStartgame.Play();
        yield return new WaitForSeconds(2);
        player.SetActive(true);
        CameraManager.instance.FollowCam(root);
        player.transform.SetParent(null);
        UIManagerController.Instance.ActivarCronometro(true);
        _starGame = true;
        yield return new WaitForSeconds(3);
        _effectStartgame.Stop();

    }
    public void DestoyPlayer() {
        Debug.Log("Destruido");
        if (TryGetComponent<PlayerControllerThirtPerson>(out PlayerControllerThirtPerson _playerController)) {
            _playerController.gameObject.SetActive(false);
        }

    }
}
