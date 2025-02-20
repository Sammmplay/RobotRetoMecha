using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControladorNavMesh : MonoBehaviour
{
    public Transform perseguirObjetivo;
    NavMeshAgent navMeshAgent;
    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void ActualizarPuntoDestinoNavMeshAgent(Vector3 puntoDestino) {
        if (navMeshAgent == null) return;
        if(puntoDestino != null) {
            navMeshAgent.destination = puntoDestino;
            navMeshAgent.isStopped = false;
        }
        
    }
    public void DetenerNavMeshAgent() {
        navMeshAgent.isStopped = true;
    }
    public void ActualizarPuntoDestinoNavMeshAgent() {
        ActualizarPuntoDestinoNavMeshAgent(perseguirObjetivo.position);
    }
    public bool hemosLLegado() {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending;

    }
}
