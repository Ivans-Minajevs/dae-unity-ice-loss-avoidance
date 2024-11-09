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
        //if (Camera.main != null)
        //{
        //    Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        //    bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //    return onScreen;
        //}
//
        //return false;
        
        var _father = FindObjectOfType<Father>();
        
        if (_father != null)
        {
            float distanceToCharacter = Vector3.Distance(transform.position, _father.transform.position);
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

