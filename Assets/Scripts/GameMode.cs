using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMode : MonoBehaviour
{
    [SerializeField] private float _firstWaveStart = 5.0f;      // Delay before the first wave
    [SerializeField] private float _waveStartFrequency = 15.0f; // Time between waves
    [SerializeField] private float _waveEndFrequency = 7.0f;    // Minimum time between waves
    [SerializeField] private float _waveFrequencyIncrement = 0.5f; // Time reduction per wave

    private float _currentFrequency;

    private const string STARTNEWWAVE_METHOD = "StartNewWave";

    void Awake()
    {
        // Set initial wave frequency
        _currentFrequency = _waveStartFrequency;

        // Start the first wave after the specified delay
        Invoke(STARTNEWWAVE_METHOD, _firstWaveStart);
    }

    // Method to trigger a new wave of spawn points
    void StartNewWave()
    {
        // Activate spawn points
        SpawnManager.Instance.SpawnWave();

        // Adjust wave frequency for the next wave
        _currentFrequency = Mathf.Clamp(
            _currentFrequency - _waveFrequencyIncrement,
            _waveEndFrequency, 
            _waveStartFrequency
        );

        // Schedule the next wave
        Invoke(STARTNEWWAVE_METHOD, _currentFrequency);
    }
}