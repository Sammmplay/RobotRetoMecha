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
    [SerializeField] float tiempoBuscando;
    private void Awake() {
        vision = GetComponent<ControladorVision>();
        navMesh = GetComponent<ControladorNavMesh>();
        maquinaDeEstados = GetComponent<StateMachine>();
    }

    private void OnEnable() {
        GameManager.Instance.SetVolume(0.2f);
        maquinaDeEstados.ChancheMaterial(2);
        maquinaDeEstados.Playsound(true, 3);
        tiempoBuscando = 0;
    }
    private void Update() {
        
        float distancePoint = Vector3.Distance(transform.position, navMesh.perseguirObjetivo.transform.position);
        RaycastHit hit;
        if(!vision.PuedeVeraLJugador(out hit , true)) {
            maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoAlerta);
            navMesh.perseguirObjetivo = null;
            maquinaDeEstados.Playsound(false, 0);
            return;
        }
        navMesh.ActualizarPuntoDestinoNavMeshAgent();
        if(distancePoint > distanceMin) {
            navMesh.ActualizarPuntoDestinoNavMeshAgent();
        } else {
            navMesh.DetenerNavMeshAgent();
        }
        if(distancePoint <= distanceMin) {
            transform.Rotate(0, velocidadGiroBusqueda * Time.deltaTime, 0);
            tiempoBuscando += Time.deltaTime;
            if(tiempoBuscando>= duracionBusqueda) {
                GameManager _script = FindObjectOfType<GameManager>();
                if (_script ) {
                    _script.DestoyPlayer();
                    maquinaDeEstados.Playsound(false, 0);
                    navMesh.perseguirObjetivo=null;
                    maquinaDeEstados.ACtivarEstado(maquinaDeEstados._estadoPatrulla);
                }
                //PlayerCombat.instance.
                return;
            }
        }
    }
}
