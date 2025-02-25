using Cinemachine;
using SD;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Transform root;
    [SerializeField] GameObject _prefabPlayer;
    [SerializeField] GameObject _playerDeadPrefab;
    [SerializeField] public GameObject player;
    [SerializeField] ParticleSystem _effectStartgame;
    public bool _starGame = false;
    [Header("ContadorEnemigos")]
    [SerializeField] int _count;
    public int _countTotal;

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
        InstantiatePlayer();
        player.SetActive(true);
        player.transform.SetParent(null);
        root = GameObject.Find("RootCamera").GetComponent<Transform>();
        CameraManager.instance.FollowCam(root);
        UIManagerController.Instance.ActivarCronometro(true);
        _starGame = true;
        yield return new WaitForSeconds(3);
        _effectStartgame.Stop();

    }
    void InstantiatePlayer() {
        player = Instantiate(_prefabPlayer, Vector3.zero, Quaternion.identity);
    }
    public void DestoyPlayer() {
        Debug.Log("Destruido");
        Destroy(player);
        GameObject _deadplayer = Instantiate(_playerDeadPrefab, player.transform.position, player.transform.rotation);
        StartCoroutine(RestarPlayer());
    }
    IEnumerator RestarPlayer() {

        yield return new WaitForSeconds(6);
        UIManagerController.Instance._panels[5].gameObject.SetActive(true);
        // Tiempo de juego
        TextMeshProUGUI _time = GameObject.Find("TimeNumText").GetComponent<TextMeshProUGUI>();
        _time.text = UIManagerController.Instance.FormatCronometro();
        //StartCoroutine(StartGame());
    }
    public void InicializarContador() {
        _countTotal++;
        UIManagerController.Instance._numEnemies.text = _count.ToString() +
            " / " + _countTotal;
    }
    public void AddContador() {
        _count++;
        UIManagerController.Instance._numEnemies.text = _count.ToString() +
            " / " + _countTotal;
        if (_count >= _countTotal) {
            Congratulations();
        }
    }
    public void RestartCountenemies() {
        _count = 0;
        _countTotal = 0;
        UIManagerController.Instance._numEnemies.text = _count.ToString() + " / "+_countTotal;
    }
    void Congratulations() {
        FindObjectOfType<PlayerControllerThirtPerson>().enabled = false;
        
        UIManagerController.Instance.ActivarCronometro(false);
        UIManagerController.Instance._isPlaying = false;
        _starGame = false;
        StartCoroutine(CongratulationsWait());

        }
    IEnumerator CongratulationsWait() {
        yield return new WaitForSeconds(2);
        FindObjectOfType<StarterAssetsInputs>().SetCursorState(false);
        UIManagerController.Instance._panels[4].gameObject.SetActive(true);
        player.transform.SetParent(this.transform);
        player.transform.localPosition = Vector3.zero;
        player.gameObject.SetActive(false);
        Button _salir = GameObject.Find("Salir").GetComponent<Button>();
        _salir.Select();
        //Puntuacion 
        TextMeshProUGUI _puntCurrent = GameObject.Find("PuntNumText").GetComponent<TextMeshProUGUI>();

        //Puntuacion Maxima 
        TextMeshProUGUI _puntMax = GameObject.Find("PuntMaxNumText").GetComponent<TextMeshProUGUI>();

        // Tiempo de juego
        TextMeshProUGUI _time = GameObject.Find("TimeNumText").GetComponent<TextMeshProUGUI>();
        _puntCurrent.text = UIManagerController.Instance.SetPuntuacion().ToString() + " Pts";
        _puntMax.text = PlayerPrefs.GetInt("MaxPunt", 0).ToString() + " Pts";
        _time.text = UIManagerController.Instance.FormatCronometro();
    }
}
