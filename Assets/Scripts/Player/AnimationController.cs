using StarterAssets;
using System;
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
    PlayerControllerThirtPerson _control;
    private void Start() {
        _char = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        _control = GetComponent<PlayerControllerThirtPerson>();
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
        } else{
            _humo.Play();
            playSound();
        }
        float circunferenciaRueda = 2 * Mathf.PI * radioRueda;
        float revolicionesXSegundo = _velocity / circunferenciaRueda;
        float gradosPorSegundo = revolicionesXSegundo * 360f;
        _rueda.transform.Rotate(_dirAngleRueda * gradosPorSegundo * Time.deltaTime);

        float velMax = _control.SprintSpeed;
        // normalizar la velocidad dentro del rango 0 -1
        float velocidadNormalizada = Mathf.InverseLerp(0, velMax, _velocity);
        _source.pitch = Mathf.Lerp(0.5f, 1, velocidadNormalizada);
        //Quaternion rotacion = Quaternion.AngleAxis(gradosPorSegundo * Time.deltaTime, Vector3.forward);
        //_rueda.transform.rotation = rotacion*_rueda.transform.rotation;
    }
    public void playSound() {
        if (!_source.isPlaying) {
            _source.Play();
        }
    }
}
