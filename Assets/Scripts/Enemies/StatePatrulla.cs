using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatrulla : MonoBehaviour
{
    public Transform[] wayPoint;
    StateMachine maquinaDeEstados;
    ControladorVision controladorVision;
    [SerializeField] int siguienteWaypoint = 0;
    ControladorNavMesh controladorNavMesh;
    NavMeshAgent _navmesh;
    [SerializeField] float distanceSlowSpeed = 5f;
    [SerializeField] float velNormal = 3f;
    public float distanciaAlWaypoint;

    float velReducida = 1f;
    private void Awake() {
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        maquinaDeEstados = GetComponent<StateMachine>();
        controladorVision = GetComponent<ControladorVision>();
        _navmesh = GetComponent<NavMeshAgent>();
        
    }
    private void Update() {

        if(controladorVision.PuedeVeraLJugador(out RaycastHit hit)) {
            controladorNavMesh.perseguirObjetivo = hit.transform;
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoPersecucion);
            return;
        }
        if (controladorNavMesh.hemosLLegado()) {
            siguienteWaypoint = (siguienteWaypoint + 1) % wayPoint.Length;
            ActualizarWayPointDestino();
        }
            distanciaAlWaypoint = Vector3.Distance(transform.position, wayPoint[siguienteWaypoint].transform.position);

        if(distanciaAlWaypoint < distanceSlowSpeed) {
            _navmesh.speed = velReducida;
        } else {
            _navmesh.speed = velNormal;
        }
        
    }
    private void OnEnable() {
        ActualizarWayPointDestino();
        maquinaDeEstados.ChancheMaterial(0);
        maquinaDeEstados.Playsound(true, 2);
    }
    void ActualizarWayPointDestino() {
        if (controladorNavMesh != null) {
            controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent(wayPoint[siguienteWaypoint].position);

        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoAlerta);
            maquinaDeEstados.Playsound(false, 0);
        }
    }
}
