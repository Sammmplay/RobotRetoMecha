using JetBrains.Annotations;
using SD;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerController : MonoBehaviour
{
    public static UIManagerController Instance;
    public RectTransform[] _panels;
    [SerializeField] Vector3 scaleButtons =  new Vector3(1.2f,1.2f,1.2f);
    [Header("Gameplay")]
    
    public TextMeshProUGUI _numEnemies;

    [Header("Cronometro")]
    [SerializeField] float _cronometro;
    float _countCronometro;
    [SerializeField] bool _activeCronometro;
    [SerializeField] TextMeshProUGUI _textCronometro;
    [Header("Puntuacion")]
    [SerializeField] int _puntuacion;
    [SerializeField] int _maxPunt;
    [SerializeField] TextMeshProUGUI _textPuntuacion;
    [Header("PausaGamplay")]
    [SerializeField] public bool _isPlaying;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        InicializarMenus();
        _countCronometro = _cronometro;
        ActualizarTextoCronometro();
#if UNITY_ANDROID
        Debug.Log("Iniciando Android");
#else
Debug.Log("Iniciando PC");
#endif
    }
    private void Start() {
        _maxPunt = PlayerPrefs.GetInt("MaxPunt", 0);

    }
    private void Update() {
        Cronometro();
        MenuPause();
    }
    
    public void InicializarMenus() {
        int indexScene = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < _panels.Length; i++) {
            _panels[i].gameObject.SetActive(false);
        }
        switch (indexScene) {
            case 0:
                _panels[0].gameObject.SetActive(true);
                break;
            case 1:
                _panels[1].gameObject.SetActive(true);
                _panels[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void Cronometro() {
        if (_activeCronometro) {
            _countCronometro -= Time.deltaTime;
            ActualizarTextoCronometro();
        }
    }
    public void ActivarCronometro(bool _active) {
        _activeCronometro = _active;
    }
    public void StopCronometro() {
        _activeCronometro = false;
        _countCronometro = _cronometro;
        ActualizarTextoCronometro();
    }
    public void ActualizarTextoCronometro() {
        _textCronometro.text = string.Format("{0:00}m:{1:00}s", Mathf.FloorToInt(_countCronometro / 60), Mathf.FloorToInt(_countCronometro % 60));
    }
    public string FormatCronometro() {
        float cronometroText = _cronometro - _countCronometro;
        
        return string.Format("{0:00}m:{1:00}s", Mathf.FloorToInt(cronometroText / 60), Mathf.FloorToInt(cronometroText % 60));
    }
    public void AddPuntuacion(int cant) {
        _puntuacion += cant;
        
        SaveMaxPunt();
    }
    void RestarPunt() {
        _puntuacion = 0;
        _textPuntuacion.text = "Puntuacion: " + _puntuacion;
    }
    public int SetPuntuacion() {
        return _puntuacion;    
    }
    #region Menu Pause.....
    void MenuPause() {
        if (Input.GetKeyDown(KeyCode.Escape)&& SceneManager.GetActiveScene().buildIndex != 0 && GameManager.Instance._starGame) {
            StarterAssetsInputs _inpu = FindObjectOfType<StarterAssetsInputs>();
            PlayerInput _plInput = FindObjectOfType<PlayerInput>();
            _panels[2].gameObject.SetActive(true);
            Button _reanudar = GameObject.Find("Reanudar").GetComponent<Button>();
            if (_inpu != null) {
                _inpu.SetCursorState(false);
            }
            if (_isPlaying) {
                if(_reanudar!= null) {
                    _reanudar.Select();
                }
               
            }
            _isPlaying = false;

            if (_plInput != null) {
                _plInput.enabled = false;
            }
            Time.timeScale = 0 ;
        }
    }
    public void Reanudar() {
        PlayerInput _plInput = FindObjectOfType<PlayerInput>();
        StarterAssetsInputs _inpu = FindObjectOfType<StarterAssetsInputs>();
        Time.timeScale = 1;
        _isPlaying = true;
        _panels[2].gameObject.SetActive(false);
#if UNITY_ANDROID
if (_plInput != null) {
_plInput.enabled = false;
        }
        
#else
        if (_plInput != null) {
_plInput.enabled = true;
        }

#endif


        if (_inpu != null) {
           _inpu.SetCursorState(true);
        }
       
    }
    public void MovePreguntaDeSeguridad(RectTransform rec) {
        LeanTween.move(rec,new Vector3(0, 356, 0),0.2f).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true);
    }
    public void RestartPreguntaDeSeguridad(RectTransform rec) {
        LeanTween.move(rec, new Vector3(0, 714, 0), 0.2f).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true).setOnComplete(() => rec.gameObject.SetActive(false));
    }

    public void Salir() { 
        LoadEscenChangeManager.instance.LoadEscene(0);
        GameManager.Instance.RestartCountenemies();
        StopCronometro();
        RestarPunt();
    }
    #endregion

    #region Animaciones de la UI
    public void ScaleButtons(RectTransform rect) {
        LeanTween.scale(rect, scaleButtons, 0.2f).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true);
    }
    public void SacleRestartButtons(RectTransform rect) {
        LeanTween.scale(rect, Vector3.one, 0.2f).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true);
    }
    #endregion

    #region Acciones botones de la UI
    public void Jugar(int index) {
        LoadEscenChangeManager.instance.LoadEscene(index);
    }
    public void Empezar() {
        GameManager.Instance.Empezar();
    }
    public void ExitApplication() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //detiene el juego en el editor de unity
#else
Application.Quit(); // cierra la aplicacion en una build
#endif
    }
    #endregion

    #region Sistema de Guardado de puntuacion 
    void SaveMaxPunt() {
        if (_puntuacion > _maxPunt) {
            _maxPunt = _puntuacion;
            PlayerPrefs.SetInt("MaxPunt", _maxPunt);
            
            Debug.Log("Max Punt Guardado");
        }
        
    }
    #endregion
}
