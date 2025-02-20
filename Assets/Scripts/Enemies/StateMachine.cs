using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public MonoBehaviour _estadoPatrulla;
    public MonoBehaviour _estadoAlerta;
    public MonoBehaviour _estadoPersecucion;
    public MonoBehaviour _estadoInicial;
    public MeshRenderer meshIndicador;
    [SerializeField] Material[] materialIndicador;
    AudioSource _source;
    public AudioClip[] _clip;
    MonoBehaviour estadoActual;

    private void Start() {
        _estadoPatrulla = GetComponent<StatePatrulla>();
        _estadoAlerta = GetComponent<StateAlerta>();
        _estadoPersecucion = GetComponent<StatePersecucion>();
        ACtivarEstado(_estadoInicial);
        GameManager.Instance.InicializarContador();
        
        _source = GetComponent<AudioSource>();
        
    }

    public void ACtivarEstado(MonoBehaviour nuevoEstado) {

        if (estadoActual != null) {
            estadoActual.enabled = false;
        }
        
        estadoActual = nuevoEstado;
        estadoActual.enabled = true;
    }
    public void ChancheMaterial(int index) {
        meshIndicador.material = materialIndicador[index];
    }
    public void Playsound(bool _play, int index) {
        if (_source == null) return;
        if (_play) {
            _source.clip = _clip[index];
            _source.Play();
        } else {
            _source.Stop();
        }
    }
}
