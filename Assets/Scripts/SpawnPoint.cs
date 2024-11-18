using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _spawnTemplate;
    private bool _hasSpawned = false;
    [SerializeField] private float _detectionRadius = 10f; // Set your desired radius here
    
    private void OnEnable()
    {
        // Register this spawn point with the SpawnManager when enabled
        SpawnManager.Instance.RegisterSpawnPoint(this);
    }

    private void OnDisable()
    {
        // Unregister this spawn point when disabled (if the manager exists)
        if (SpawnManager.Exists) SpawnManager.Instance.UnRegisterSpawnPoint(this);
    }
    private void Update()
    {
        if (IsInView() && !_hasSpawned)
        {
            SpawnZombie();
        }
    }

    // Check if the spawn point is within the camera's view
    private bool IsInView()
    {
        var father = FindObjectOfType<Father>();
        
        if (father != null)
        {
            float distanceToCharacter = Vector3.Distance(transform.position, father.transform.position);
            return distanceToCharacter <= _detectionRadius;
        }

        return false;
    }

    private void SpawnZombie()
    {
        _hasSpawned = true; // Mark as spawned
        Spawn();            // Spawn the zombie
        DestroySpawnPoint(); 
    }

    public GameObject Spawn()
    {
        return Instantiate(_spawnTemplate, transform.position, transform.rotation);
    }

    private void DestroySpawnPoint()
    {
        Destroy(gameObject); // Destroy the spawn point object
    }
}

