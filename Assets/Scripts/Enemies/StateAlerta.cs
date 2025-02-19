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
    Vector3 ultimoPuntoDetectado;
    private void Awake() {
        maquinaDeEstados = GetComponent<StateMachine>();
        controladorVision = GetComponent<ControladorVision>();
        navMesh = GetComponent<ControladorNavMesh>();
    }
    private void OnEnable() {
        maquinaDeEstados.ChancheMaterial(1);
        navMesh.DetenerNavMeshAgent();
        
        maquinaDeEstados.Playsound(true, 1);
        tiempoBuscando = 0;
    }
    private void Update() {
        if (controladorVision.PuedeVeraLJugador(out RaycastHit hit)) {
            navMesh.perseguirObjetivo = hit.transform;
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoPersecucion);
            return;
        }
        if(ultimoPuntoDetectado!=Vector3.zero){
        //calcular la direccion hacia el jugador
        Vector3 direccionHaciaEljugador = (ultimoPuntoDetectado - transform.position);
        direccionHaciaEljugador.y = 0;
        //Rotar suavemente hacia la ultima posicion detectada
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionHaciaEljugador);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiroBusqueda * Time.deltaTime);
        }
        tiempoBuscando += Time.deltaTime;
        if(tiempoBuscando >= duracionBusqueda) {
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoPatrulla);
            maquinaDeEstados.Playsound(false, 0);
            return;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            ultimoPuntoDetectado = other.transform.position;
            return;
        } else {
            ultimoPuntoDetectado = Vector3.zero;
        }
    }
}
