using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public MonoBehaviour _estadoPatrulla;
    public MonoBehaviour _estadoAlerta;
    public MonoBehaviour _estadoPersecucion;

    public MonoBehaviour _estadoInicial;

    MonoBehaviour estadoActual;

    private void Start() {
        _estadoPatrulla = GetComponent<StatePatrulla>();
        _estadoAlerta = GetComponent<StateAlerta>();
        _estadoPersecucion = GetComponent<StatePersecucion>();
        ACtivarEstado(_estadoInicial);
    }

    public void ACtivarEstado(MonoBehaviour nuevoEstado) {

        if (estadoActual!=null) {
            estadoActual.enabled = false;
        }
        
        estadoActual = nuevoEstado;
        estadoActual.enabled = true;
    }
}
