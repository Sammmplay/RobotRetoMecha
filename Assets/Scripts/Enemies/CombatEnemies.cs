using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnemies : MonoBehaviour
{
    [Header("Puntuacion")]
    public int _punt;
    [Header("Muerte")]
    
    [SerializeField] GameObject[] _efects;
    public void Dead() {
        UIManagerController.Instance.AddPuntuacion(_punt);
        GameManager.Instance.AddContador();
        
        if (_efects != null) {
            GameObject _efect = Instantiate(_efects[0], transform.position, Quaternion.identity);
            
        }
        StateMachine _state = GetComponent<StateMachine>();
        _state.enabled = false;
        
        Destroy(gameObject);
    }
    
}
