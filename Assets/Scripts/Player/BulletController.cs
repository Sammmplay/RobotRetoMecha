using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody _rb;
    Transform _targetEnemie;
    [SerializeField] float _speed = 10f; // velocidad de la bala
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _frecuency=5f; // frecuencia de oscilacion
    [SerializeField] float magnitude = 0.5f; // Amplitud de oscilacion
    [SerializeField] float _distanceEnemi;

    Vector3 _dir;
    Vector3 starPosition;
    private void Start() {
        _rb = GetComponent<Rigidbody>();
        
        starPosition = transform.position;
    }
    private void OnEnable() {
        transform.SetParent(null);
        Destroy(gameObject, 10);
    }
    private void Update() {
        if(_targetEnemie == null) {
            _rb.velocity += _dir * _speed * Time.deltaTime;
            //movimiento oscilatorio en el eje x 
            _rb.velocity += transform.right * Mathf.Sin(Time.time * _frecuency) * magnitude;
        } else {
            Vector3 targetDireccion = (_targetEnemie.position - transform.position).normalized;
            //rotar el misil suavemente hacia el enemigo 
            Quaternion targetRotation = Quaternion.LookRotation(targetDireccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            // movimiento oscilatorio para dar efecto de fisil realista
            Vector3 oscillation = transform.right * Mathf.Sin(Time.time * _frecuency) * magnitude;
            //aplicar la velocidad en la direccion del misil + oscilacion
            _rb.velocity = (transform.forward * _speed) + oscillation;
        }
    }
    public void TransformDirection(Vector3 dir) {
        _dir = dir;
    }
    public void DetectTransform(Transform enemie) {
        _targetEnemie = enemie;

    }
}
