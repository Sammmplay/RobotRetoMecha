using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class AnimationController : MonoBehaviour
{
    Animator _anim;
    StarterAssetsInputs _input;
    CharacterController _char;
    AudioSource _source;
    [Header("AnimRueda")]
    public Transform _rueda;
    [SerializeField] Vector3 _dirAngleRueda;

    [SerializeField] float _velocity;
    [SerializeField] float radioRueda;
    [Header("AnimSmoke")]
    [SerializeField] ParticleSystem _humo;

    private void Start() {
        _char = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
    }
    private void Update() {
        _velocity = _char.velocity.magnitude;
        AnimacionEjeRueda();
        AnimacionUp();
    }
    void AnimacionUp() {
        _anim.SetFloat("Velocity", _velocity);

    }
    public void AnimacionEjeRueda() {
        if (_velocity < 0.1f) {
            _source.Stop();
            //_humo.Stop();
        } else {
            _humo.Play();
            //_source.Play();
        }
        float circunferenciaRueda = 2 * Mathf.PI * radioRueda;
        float revolicionesXSegundo = _velocity / circunferenciaRueda;
        float gradosPorSegundo = revolicionesXSegundo * 360f;
        _rueda.transform.Rotate(_dirAngleRueda * gradosPorSegundo * Time.deltaTime);
       
        //Quaternion rotacion = Quaternion.AngleAxis(gradosPorSegundo * Time.deltaTime, Vector3.forward);
        //_rueda.transform.rotation = rotacion*_rueda.transform.rotation;
    }
}
