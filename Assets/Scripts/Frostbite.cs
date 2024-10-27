using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frostbite: MonoBehaviour
{
    [SerializeField]
    private int _startFrostbite = 0;
    private int _currentFrostbite = 0;
    private const int _maxFrostbite = 100;
    public float MaxFrostbite { get { return _maxFrostbite; } }
    public float CurrentFrostbite { get { return _currentFrostbite; } }
    public delegate void FrostbiteChange(float maxFrostbite, float currentFrstbite);
    public event FrostbiteChange OnFrostbiteChanged;
    void Awake()
    {
        _currentFrostbite = _startFrostbite;
    }
    public void Freeze(int amount)
    {
        _currentFrostbite += amount;
        OnFrostbiteChanged?.Invoke(_maxFrostbite, _currentFrostbite);
        if (_currentFrostbite >= _maxFrostbite) Kill();
    }

    public void Heat(int amount)
    {
        _currentFrostbite -= amount;
        if (_currentFrostbite <= 0) _currentFrostbite = 0;
        OnFrostbiteChanged?.Invoke(_maxFrostbite, _currentFrostbite);
        
    }
    void Kill()
    {
        Destroy(gameObject);
    }
}

