using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class AnimationController : MonoBehaviour
{
    public Transform _rueda;
    Animator _anim;
    StarterAssetsInputs _input;
    CharacterController _char;
    [SerializeField] Vector2 _dir;

    [SerializeField] float _velocity;
    [SerializeField] float radioRueda;
    private void Start() {
        _char = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _anim = GetComponent<Animator>();
    }
    private void Update() {
        _velocity = _char.velocity.magnitude;
        AnimacionEjeRueda();
        AnimacionUp();
    }
    void AnimacionUp() {
        _dir = _input.move;
        _anim.SetFloat("Velocity", _velocity);
        _anim.SetFloat("DirX", _dir.x);
        _anim.SetFloat("DirY", _dir.y);
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
