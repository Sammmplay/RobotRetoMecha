using UnityEngine;

public class CombatEnemies : MonoBehaviour
{
    [Header("Puntuacion")]
    public int _punt;
    [Header("Muerte")]
    [SerializeField] float percentInstantMaxCharge = 10;
    [SerializeField] float _percentMaxChargeLongTime = 5f;
    [SerializeField] GameObject[] _efects;
    [SerializeField] GameObject _powerUp;
    private void Start() {
        percentInstantMaxCharge /= 100;
        _percentMaxChargeLongTime /= 100;
    }
    public void Dead() {
        UIManagerController.Instance.AddPuntuacion(_punt);
        GameManager.Instance.AddContador();
        float percent = Random.Range(0, 1);
        if (_efects != null) {
            GameObject _efect = Instantiate(_efects[0], transform.position, Quaternion.identity);
        }
        StateMachine _state = GetComponent<StateMachine>();
        _state.enabled = false;
        if (percent <= percentInstantMaxCharge) {
            Instantiate(_powerUp, transform.position, Quaternion.identity);
            _powerUp.GetComponent<PowerUps>().id = 0;

        } else if (percent <= _percentMaxChargeLongTime) {
            _powerUp.GetComponent<PowerUps>().id = 1;
            Instantiate(_powerUp, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);

    }
    
}
