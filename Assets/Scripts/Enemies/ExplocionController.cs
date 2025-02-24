using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplocionController : MonoBehaviour
{
    [SerializeField] float _expForce = 100f;
    [SerializeField] float _expRadius = 2f;
    [SerializeField] float _offset;
    [SerializeField] float _timeDestroy;
    private void Start() {
        Explosion();
    }
    void Explosion() {
        Destroy(gameObject,_timeDestroy);
        Rigidbody[] _rb = GetComponentsInChildren<Rigidbody>();
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - _offset, transform.position.z);
        for (int i = 0; i < _rb.Length; i++) {
            _rb[i].isKinematic = false;

            _rb[i].AddExplosionForce(_expForce, pos, _expRadius);
        }
    }
}
