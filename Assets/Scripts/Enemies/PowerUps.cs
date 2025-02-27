using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public float id;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (id == 0) {
                Debug.Log("MaxCharge");
                other.GetComponent<EnergySystem>().MaxCharge();
                Destroy(gameObject,0.5f);
            } 
            if (id == 1) {
                Debug.Log("ApplyEfect");
                other.GetComponent<EnergySystem>().AplyEfect();
                Destroy(gameObject);
            }
            
        }
    }
    
}
