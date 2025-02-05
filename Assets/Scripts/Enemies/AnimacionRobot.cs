using UnityEngine;
using UnityEngine.AI;

public class AnimacionRobot : MonoBehaviour
{
    [Header("AnimacionRueda")]
    [SerializeField] GameObject _rueda;
    [SerializeField] float radioRueda = 0.00576233f;
    [Header("Animacion Cuerpo")]
    [SerializeField] Transform _cuerpoSuperior;
    [SerializeField] float inclinacionMaxima = 30f;
    [SerializeField] float velocidadDeInclinacion = 5f;

    [SerializeField] Vector3 directionNavmesh;
    Animator _anim;
    [SerializeField] float velocidad;
    NavMeshAgent _navMesh;
    private void Start() {
        _navMesh = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }
    private void Update() {
        velocidad = _navMesh.velocity.magnitude;
        AnimRueda();
        Animaciones();
    }
    void AnimRueda() {
        
        float circunferenciaRueda = 2 * Mathf.PI * radioRueda;
        float revolicionesXSegundo = velocidad / circunferenciaRueda;
        float gradosPorSegundo = revolicionesXSegundo * 360f;
        _rueda.transform.Rotate(Vector3.right * gradosPorSegundo * Time.deltaTime);
    }

    
    public void Animaciones() {
        directionNavmesh = _navMesh.velocity.normalized;
        if(_navMesh.speed > 3 && _navMesh.speed<3.5f) {
            _anim.SetBool("Acelerando", true);
        } else if (_navMesh.speed>=3.5f) {
            _anim.SetBool("Acelerando", false);
        }
    }
}
