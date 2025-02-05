using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAlerta : MonoBehaviour
{
    public float velocidadGiroBusqueda = 120f;
    public float duracionBusqueda = 4f;
    float tiempoBuscando;
    StateMachine maquinaDeEstados;
    ControladorVision controladorVision;
    ControladorNavMesh navMesh;
    private void Awake() {
        maquinaDeEstados = GetComponent<StateMachine>();
        controladorVision = GetComponent<ControladorVision>();
        navMesh = GetComponent<ControladorNavMesh>();
    }
    private void OnEnable() {
        maquinaDeEstados.ChancheMaterial(1);
        navMesh.DetenerNavMeshAgent();
        tiempoBuscando = 0;
    }
    private void Update() {
        if (controladorVision.PuedeVeraLJugador(out RaycastHit hit)) {
            navMesh.perseguirObjetivo = hit.transform;
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoPersecucion);
            return;
        }
        transform.Rotate(0, velocidadGiroBusqueda * Time.deltaTime, 0);    
        tiempoBuscando += Time.deltaTime;
        if(tiempoBuscando >= duracionBusqueda) {
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoPatrulla);
            return;
        }
    }
}
