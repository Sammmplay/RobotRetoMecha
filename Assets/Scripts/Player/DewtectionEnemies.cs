using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DewtectionEnemies : MonoBehaviour
{
    [SerializeField] PlayerControllerThirtPerson _playerC;
    [SerializeField] float detectionRange = 15f;
    [SerializeField] float capsuleRadius = 5f;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer; // Capa para las paredes u obstáculos

    private List<CombatEnemies> currentlyDetectedEnemies = new List<CombatEnemies>(); // Lista para almacenar enemigos detectados

    [SerializeField,Range(0,1)] float volume = 1;
    bool isPlay;
    private void Start() {
        _playerC = GetComponentInParent<PlayerControllerThirtPerson>();
    }
    private void Update() {
        DetectEnemies();
        
        if (_playerC.enemies.Count > 0) {
            GameManager.Instance.SetVolume(volume);
        } else {
            GameManager.Instance.SetVolume(1);
        }
    }
    void DetectEnemies() {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * detectionRange;

        // Primero, eliminamos a los enemigos que ya no están en el rango
        List<CombatEnemies> enemiesToRemove = new List<CombatEnemies>();

        // Verificamos todos los enemigos actualmente detectados
        foreach (CombatEnemies enemy in currentlyDetectedEnemies) {
            if (enemy == null || !IsEnemyInDetectionRange(enemy)) {
                // Si el enemigo ya no está dentro del rango de detección o ha sido destruido, lo marcamos para eliminar
                enemiesToRemove.Add(enemy);
            }
        }

        // Eliminamos los enemigos que ya no están detectados
        foreach (CombatEnemies enemy in enemiesToRemove) {
            currentlyDetectedEnemies.Remove(enemy);
            _playerC.RemoveEnemies(enemy); // Eliminamos del sistema de gestión de enemigos
        }

        // Ahora, lanzamos el CapsuleCast para encontrar nuevos enemigos dentro del rango
        RaycastHit[] hits = Physics.CapsuleCastAll(
            startPosition,
            endPosition,
            capsuleRadius,
            transform.forward,
            detectionRange,
            enemyLayer
        );

        foreach (RaycastHit hit in hits) {
            CombatEnemies enemy = hit.collider.GetComponent<CombatEnemies>();
            if (enemy != null && !currentlyDetectedEnemies.Contains(enemy)) {
                // Si el enemigo no está ya en la lista de enemigos detectados, lo agregamos
                currentlyDetectedEnemies.Add(enemy);
                _playerC.AddEnemies(enemy); // Añadimos al sistema de gestión de enemigos
            }
        }
    }

    // Verifica si un enemigo está dentro del rango de detección
    bool IsEnemyInDetectionRange(CombatEnemies enemy) {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * detectionRange;

        // Verificamos si el enemigo está dentro del rango de visión y no hay obstáculos entre ellos
        RaycastHit hit;
        if (Physics.Raycast(startPosition, (enemy.transform.position - startPosition).normalized, out hit, detectionRange, obstacleLayer)) {
            if (hit.collider.gameObject != enemy.gameObject) {
                // Si el raycast golpea algo que no es el enemigo, significa que hay un obstáculo en el camino
                return false;
            }
        }

        return true; // Si el enemigo está en el rango y no hay obstáculos, lo consideramos como detectado
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        // Definir las posiciones de la cápsula
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * detectionRange;

        // Dibujar una línea central para visualizar la dirección
        Gizmos.DrawLine(startPosition, endPosition);

        // Dibujar las esferas en los extremos de la cápsula
        Gizmos.DrawWireSphere(startPosition, capsuleRadius);
        Gizmos.DrawWireSphere(endPosition, capsuleRadius);
    }
}
    /*void DetectEnemies() {
        Vector3 starPosition = transform.position;
        Vector3 endPosition = starPosition + transform.forward * detectionRange;

        //Lanazamos el capsuleCast
        RaycastHit[] hits = Physics.CapsuleCastAll(
            starPosition,
            endPosition,
            capsuleRadius,
            transform.forward,
            detectionRange,
            enemylayer);
        List<CombatEnemies> detectEnemiesInFrame = new List<CombatEnemies>();
        foreach (RaycastHit hit in hits) {
            CombatEnemies enemy = hit.collider.GetComponent<CombatEnemies>();
            if (enemy != null) {
                detectEnemiesInFrame.Add(enemy);

                if (!_playerC.enemies.Contains(enemy)) {
                    _playerC.AddEnemies(enemy);

                }
            }
        }
        for (int i = 0; i < _playerC.enemies.Count-1; i++) {
            if (!detectEnemiesInFrame.Contains(_playerC.enemies[i])) {
                _playerC.RemoveEnemies(_playerC.enemies[i]);
            }
        }
        
    }*/

   

