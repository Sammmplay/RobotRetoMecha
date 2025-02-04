using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrulla : MonoBehaviour
{
    public Transform[] wayPoint;
    [SerializeField] int siguienteWaypoint = 0;
    ControladorNavMesh controladorNavMesh;
    private void Awake() {
        controladorNavMesh = GetComponent<ControladorNavMesh>();
    }
    private void Update() {
        if (controladorNavMesh.hemosLLegado()) {
            siguienteWaypoint = (siguienteWaypoint + 1) % wayPoint.Length;
            ActualizarWayPointDestino();
        }
    }
    private void OnEnable() {
        ActualizarWayPointDestino();

    }
    void ActualizarWayPointDestino() {
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent(wayPoint[siguienteWaypoint].position);
    }
}
