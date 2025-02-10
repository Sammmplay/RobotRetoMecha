using SD;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerController : MonoBehaviour
{
    public static UIManagerController Instance;
    public RectTransform[] _panels;
    public GameObject _jostycs;
    [SerializeField] Vector3 scaleButtons =  new Vector3(1.2f,1.2f,1.2f);
    [Header("Gameplay")]
    [SerializeField] GameObject _numEnemiesGO;
    public TextMeshProUGUI _numEnemies;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        InicializarMenus();
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
                if(Application.platform == RuntimePlatform.WindowsPlayer || 
                    Application.platform == RuntimePlatform.WindowsEditor) {
                    Debug.Log("Juego Ejecutandose En PC");
                    _jostycs.SetActive(false);
                    
                }else if (Application.platform == RuntimePlatform.Android) {
                    Debug.Log("Juego ejecutandose en Android");
                    _jostycs.SetActive(true);
                }
                _panels[1].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void ScaleButtons(RectTransform rect) {
        LeanTween.scale(rect, scaleButtons, 0.2f).setEase(LeanTweenType.easeInBack);
    }
    public void SacleRestartButtons(RectTransform rect) {
        LeanTween.scale(rect, Vector3.one, 0.2f).setEase(LeanTweenType.easeInBack);
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
}
