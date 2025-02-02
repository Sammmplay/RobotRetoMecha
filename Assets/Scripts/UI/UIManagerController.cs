using SD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerController : MonoBehaviour
{
    public static UIManagerController Instance;
    [SerializeField] Vector3 scaleButtons =  new Vector3(1.2f,1.2f,1.2f);

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
