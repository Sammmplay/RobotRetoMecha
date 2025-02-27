using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    
    [SerializeField] Slider _energy;
    [SerializeField] float _valuefireEnergy;
    [SerializeField] float _maxEnergy;
    [SerializeField] float _timereloading;
    [SerializeField] float percentCharge;
    [SerializeField] float timeLongApplyEfect = 30f;
    float time_max_Charge;
    float progres;
    public bool canReloading = true;
    [SerializeField] bool _applyEfect;
    float time; 
    private void Awake() {
        _energy = GameObject.Find("EnergyBarra").GetComponent<Slider>();
        if(_energy != null) {
            _energy.maxValue = _maxEnergy;
            _energy.value = _maxEnergy;
        }
        
    }
    private void Update() {
        ReloadEnergy();
        EnergymaxLongTime();
    }
    public void ChancheEnergy() {
        if (_energy.value > _valuefireEnergy) {
            _energy.value -= _valuefireEnergy;
        }
    }
    public bool canAttack() {
        return _energy.value > _valuefireEnergy;
    }
    public void ReloadEnergy() {
        if (canReloading) {
            time += Time.deltaTime;
            if (time > _timereloading) {
                progres += Time.deltaTime;
                if (progres>=1) {
                    if (_energy.value < _maxEnergy) {
                        float percent = ((percentCharge / 100) * _maxEnergy);
                        //_energy.value = Mathf.Clamp(percent, 0, _maxEnergy);
                        _energy.value += percent;
                        progres = 0;
                    }
                    
                }
                
            }
        }
    }
    public void MaxCharge() {
        _energy.value = _maxEnergy;
    }
    void EnergymaxLongTime() {
        if (_applyEfect) {
            _energy.value = _maxEnergy;
            time_max_Charge += Time.deltaTime;
            if (time_max_Charge >= timeLongApplyEfect) {
                _applyEfect = false;
            }
        }
    }
    public void AplyEfect() {
        if (_applyEfect) {
            Debug.Log("Ya tiene El efecto Aplicado");
            time_max_Charge = 0;
        } else {
            Debug.Log("No tiene efecto alguno");
            _applyEfect = true;
        }
    }
}
