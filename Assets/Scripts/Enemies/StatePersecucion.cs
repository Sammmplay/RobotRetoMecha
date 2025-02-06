using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePersecucion : MonoBehaviour
{
    ControladorVision vision;
    ControladorNavMesh navMesh;
    StateMachine maquinaDeEstados;
    [SerializeField] float velocidadGiroBusqueda = 50f;
    [SerializeField] float duracionBusqueda = 4f;
    [SerializeField] float distanceMin=8.0f;
    float tiempoBuscando;
    private void Awake() {
        vision = GetComponent<ControladorVision>();
        navMesh = GetComponent<ControladorNavMesh>();
        maquinaDeEstados = GetComponent<StateMachine>();
    }

    private void OnEnable() {
        maquinaDeEstados.ChancheMaterial(2);
    }
    private void Update() {
        float distancePoint = Vector3.Distance(transform.position, navMesh.perseguirObjetivo.transform.position);
        RaycastHit hit;
        if(!vision.PuedeVeraLJugador(out hit , true)) {
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoAlerta);
            navMesh.perseguirObjetivo = null;
            return;
        }
        navMesh.ActualizarPuntoDestinoNavMeshAgent();
        if(distancePoint > distanceMin) {
            navMesh.ActualizarPuntoDestinoNavMeshAgent();
        } else {
            navMesh.DetenerNavMeshAgent();
        }
        if(distancePoint<= distanceMin) {
            transform.Rotate(0, velocidadGiroBusqueda * Time.deltaTime, 0);
            tiempoBuscando += Time.deltaTime;
            if(tiempoBuscando>= duracionBusqueda) {
                Debug.Log("HasMuerto");
                return;
            }
        }
    }
}
