using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountEnemies : MonoBehaviour
{
    public static CountEnemies Instance;
    [SerializeField] int _count;
    public int _countTotal;
    private void Start() {
        Instance = this;
    }
    public void InicializarContador() {
        _countTotal++;
        UIManagerController.Instance._numEnemies.text = _count.ToString() +
            " / " + _countTotal;
    }
    public void AddContador() {
        _count++;
        UIManagerController.Instance._numEnemies.text = _count.ToString() +
            " / " + _countTotal;
    }
}
