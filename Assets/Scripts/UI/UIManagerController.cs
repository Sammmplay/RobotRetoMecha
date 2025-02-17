using SD;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerController : MonoBehaviour
{
    public static UIManagerController Instance;
    public RectTransform[] _panels;
    [SerializeField] Vector3 scaleButtons =  new Vector3(1.2f,1.2f,1.2f);
    [Header("Gameplay")]
    
    public TextMeshProUGUI _numEnemies;

    [Header("Cronometro")]
    [SerializeField] float _cronometro;
    
    [SerializeField] bool _activeCronometro;
    [SerializeField] TextMeshProUGUI _textCronometro;
    [Header("Puntuacion")]
    [SerializeField] int _puntuacion;
    [SerializeField] int _maxPunt;
    [SerializeField] TextMeshProUGUI _textPuntuacion;
    [Header("PausaGamplay")]
    [SerializeField] bool _isPlaying;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        InicializarMenus();
        
        ActualizarTextoCronometro();
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
                break;
            default:
                break;
        }
    }
    
    public void Cronometro() {
        if (_activeCronometro) {
            _cronometro -= Time.deltaTime;
            ActualizarTextoCronometro();
        }
    }
    void ActualizarTextoCronometro() {
        _textCronometro.text = string.Format("{0:00}m:{1:00}s", Mathf.FloorToInt(_cronometro / 60), Mathf.FloorToInt(_cronometro % 60));
    }
    public void AddPuntuacion(int cant) {
        _puntuacion += cant;
        _textPuntuacion.text = "Puntuacion: " + _puntuacion;
        SaveMaxPunt();
    }

    void MenuPause() {
        if(Input.GetKeyDown(KeyCode.Escape))
        _isPlaying = !_isPlaying;
        _panels[2].gameObject.SetActive(_isPlaying);
        Time.timeScale = _isPlaying ? 0 : 1;
    }
    public void ScaleButtons(RectTransform rect) {
        LeanTween.scale(rect, scaleButtons, 0.2f).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true);
    }
    public void SacleRestartButtons(RectTransform rect) {
        LeanTween.scale(rect, Vector3.one, 0.2f).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true);
    }
    public void Jugar() {
        LoadEscenChangeManager.instance.LoadEscene(1);
    }
    public void ExitApplication() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //detiene el juego en el editor de unity
#else
Application.Quit(); // cierra la aplicacion en una build
#endif
    }

    #region Sistema de Guardado de puntuacion 
    void SaveMaxPunt() {
        if (_puntuacion > _maxPunt) {
            PlayerPrefs.SetInt("MaxPunt", _maxPunt);
        }
        
    }
    #endregion
}
