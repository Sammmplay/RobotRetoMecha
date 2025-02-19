using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BulletController : MonoBehaviour
{
    Rigidbody _rb;
    Transform _targetEnemie;
    [SerializeField] float _speed = 10f; // velocidad de la bala
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _frecuency=5f; // frecuencia de oscilacion
    [SerializeField] float magnitude = 0.5f; // Amplitud de oscilacion
    [Header("BallDestroy")]
    [SerializeField] float totalDistance = 0;
    [SerializeField] float distanceMax = 15;
    [SerializeField] GameObject _efectsCollision;
    Vector3 _dir;
    Vector3 starPosition;
    private void Start() {
        _rb = GetComponent<Rigidbody>();
        Collider _col = GetComponent<Collider>();
        _col.enabled = true;
        Debug.Log("starBullet");
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
        float distanceThisFrame = Vector3.Distance(starPosition, transform.position);
        totalDistance = distanceThisFrame;
        if (totalDistance >= distanceMax) {
            Destroy(gameObject);
        }
    }
    public void TransformDirection(Vector3 dir) {
        _dir = dir;
    }
    public void DetectTransform(Transform enemie) {
        _targetEnemie = enemie;

    }
    private void OnTriggerEnter(Collider other) {
        
        if (!other.CompareTag("Player") && !other.isTrigger) {
            // Instanciar el efecto en la posición del objeto colisionado
            GameObject impactEffect = Instantiate(_efectsCollision, other.transform.position, Quaternion.identity);

            // Obtener el color del material del objeto impactado
            MeshRenderer hitRenderer = other.GetComponent<MeshRenderer>();
            if (hitRenderer != null && hitRenderer.materials.Length > 0) {
                Color hitColor = GetClosestMaterialColor(hitRenderer);
                ChangeParticleColor(impactEffect, hitColor);
            }
            if (other.TryGetComponent<CombatEnemies>(out CombatEnemies script)) {
                script.Dead();
            }
            Destroy(gameObject);
        }
        
    }

    // Método para obtener el color del material más cercano (sin contacto exacto)
    private Color GetClosestMaterialColor(MeshRenderer renderer) {
        if (renderer.materials.Length == 1) {
            return renderer.materials[0].color;
        }
        int randomIndex = Random.Range(0, renderer.materials.Length);
        return renderer.materials[randomIndex].color;
    }

    // Cambiar el color de las partículas del efecto instanciado
    private void ChangeParticleColor(GameObject effectInstance, Color newColor) {
        var particleSystem = effectInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null) {
            var mainModule = particleSystem.main;

            // Crear un gradiente desde blanco al color detectado
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(newColor, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
            );

            mainModule.startColor = new ParticleSystem.MinMaxGradient(gradient);
        }
    }
}
