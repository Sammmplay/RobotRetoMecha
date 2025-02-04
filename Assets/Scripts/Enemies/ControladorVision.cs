using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorVision : MonoBehaviour
{
    public Transform _eyes;
    public float rangoVision = 20f;
    public Vector3 offset = new Vector3(0, 0.5f, 0.0f);
    ControladorNavMesh controloNavMeshAgent;
    private void Start() {
        controloNavMeshAgent = GetComponent<ControladorNavMesh>();
    }
    public bool PuedeVeraLJugador(out RaycastHit hit, bool mirarHAciaElJugador = false) {
        Vector3 vectorDirection;
        if (mirarHAciaElJugador) {
            vectorDirection = (controloNavMeshAgent.perseguirObjetivo.position + offset) - _eyes.position;
        } else {
            vectorDirection = _eyes.forward;
        }
        return Physics.Raycast(_eyes.position, vectorDirection, out hit, rangoVision) &&
            hit.collider.CompareTag("Player");
    }
}
