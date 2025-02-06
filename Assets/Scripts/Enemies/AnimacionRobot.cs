using UnityEngine;
using UnityEngine.AI;

public class AnimacionRobot : MonoBehaviour
{
    StatePatrulla _statePatrulla;
    NavMeshAgent _navMesh;
    Animator _anim;
    ControladorNavMesh _navMeshControlador;
    [Header("AnimacionRueda")]
    [SerializeField] GameObject _rueda;
    [SerializeField] float radioRueda = 0.00576233f;
    [Header("Animacion Cuerpo")]
    
    [SerializeField] Vector3 directionNavmesh;
    
    [SerializeField] float velocidad;
    [SerializeField] float _distanceBrake;
    [SerializeField] bool _isBrake;
    [SerializeField] bool _isAcceleration;
    private void Start() {
        _navMesh = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _statePatrulla = GetComponent<StatePatrulla>();
        _navMeshControlador = GetComponent<ControladorNavMesh>();
    }
    private void Update() {
        velocidad = _navMesh.velocity.magnitude;
        _anim.SetFloat("Velocidad", velocidad);
        AnimRueda();
        Animaciones();
    }
    void AnimRueda() {
        
        float circunferenciaRueda = 2 * Mathf.PI * radioRueda;
        float revolicionesXSegundo = velocidad / circunferenciaRueda;
        float gradosPorSegundo = revolicionesXSegundo * 360f;
        _rueda.transform.Rotate(Vector3.right * gradosPorSegundo * Time.deltaTime);
    }

    
    public void Animaciones() { //animacines en estado persecucion
        directionNavmesh = _navMesh.velocity.normalized;
        if (_statePatrulla.distanciaAlWaypoint <= _distanceBrake && _statePatrulla.distanciaAlWaypoint>=1f) {
            _isBrake = true;
        }
        if (_navMeshControlador.hemosLLegado()) {
            _isBrake = false;
            _isAcceleration = true;
            
        }else if (velocidad >= 3.5f) {
            _isAcceleration = false;
        }
        _anim.SetBool("Frenado", _isBrake);
        _anim.SetBool("Acelerando", _isAcceleration);
    }
}
