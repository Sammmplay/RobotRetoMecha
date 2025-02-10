using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Transform _rueda;
    CharacterController _char;
    [SerializeField] float _velocity;
    [SerializeField]float radioRueda;
    private void Start() {
        _char = GetComponent<CharacterController>();
    }
    private void Update() {
        _velocity = _char.velocity.magnitude;
        AnimacionEjeRueda();
    }

    public void AnimacionEjeRueda() {
        float circunferenciaRueda = 2 * Mathf.PI * radioRueda;
        float revolicionesXSegundo = _velocity / circunferenciaRueda;
        float gradosPorSegundo = revolicionesXSegundo * 360f;
        _rueda.transform.Rotate(Vector3.forward * gradosPorSegundo * Time.deltaTime);
        //Quaternion rotacion = Quaternion.AngleAxis(gradosPorSegundo * Time.deltaTime, Vector3.forward);
        //_rueda.transform.rotation = rotacion*_rueda.transform.rotation;
    }
}
