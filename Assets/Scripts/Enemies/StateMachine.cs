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
    MonoBehaviour estadoActual;

    private void Start() {
        _estadoPatrulla = GetComponent<StatePatrulla>();
        _estadoAlerta = GetComponent<StateAlerta>();
        _estadoPersecucion = GetComponent<StatePersecucion>();
        ACtivarEstado(_estadoInicial);
        CountEnemies.Instance.InicializarContador();
    }

    public void ACtivarEstado(MonoBehaviour nuevoEstado) {

        if (estadoActual!=null) {
            estadoActual.enabled = false;
        }
        
        estadoActual = nuevoEstado;
        estadoActual.enabled = true;
    }
    public void ChancheMaterial(int index) {
        meshIndicador.material = materialIndicador[index];
    }
}
