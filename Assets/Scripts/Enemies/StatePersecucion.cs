using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePersecucion : MonoBehaviour
{
    ControladorVision vision;
    ControladorNavMesh navMesh;
    StateMachine maquinaDeEstados;
    private void Awake() {
        vision = GetComponent<ControladorVision>();
        navMesh = GetComponent<ControladorNavMesh>();
        maquinaDeEstados = GetComponent<StateMachine>();
    }

    private void OnEnable() {
        maquinaDeEstados.ChancheMaterial(2);
    }
    private void Update() {
        RaycastHit hit;
        if(!vision.PuedeVeraLJugador(out hit , true)) {
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoAlerta);
            return;
        }
        //navMesh.ActualizarPuntoDestinoNavMeshAgent();

    }
}
