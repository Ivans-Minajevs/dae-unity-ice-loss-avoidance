using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy: MonoBehaviour
{
    [SerializeField]
    private int _startEnergy= 100;
    private int _currentEnergy = 0;
    public float StartEnergy { get { return _startEnergy; } }
    public float CurrentEnergy { get { return _currentEnergy; } }
    public delegate void EnergyChange(float startHealth, float currentHealth);
    public event EnergyChange OnEnergyChanged;
    void Awake()
    {
        _currentEnergy = _startEnergy;
    }
    public void Tire(int amount)
    {
        if (_currentEnergy >= 5)
        {
            _currentEnergy -= amount;
        }
        
        OnEnergyChanged?.Invoke(_startEnergy, _currentEnergy);
    }
}
