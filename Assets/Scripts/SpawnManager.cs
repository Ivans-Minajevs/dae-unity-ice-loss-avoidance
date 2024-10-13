using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region SINGLETON INSTANCE

    private static SpawnManager _instance;

    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null && !ApplicationQuitting)
            {
                _instance = FindObjectOfType<SpawnManager>();
                if (_instance == null)
                {
                    GameObject newInstance = new GameObject("Singleton_SpawnManager");
                    _instance = newInstance.AddComponent<SpawnManager>();
                }
            }

            return _instance;
        }
    }

    public static bool Exists
    {
        get
        {
            return _instance != null;
        }
    }

    public static bool ApplicationQuitting = false;

    protected virtual void OnApplicationQuit()
    {
        ApplicationQuitting = true;
    }

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    // List to store registered spawn points
    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

    // Register a new spawn point
    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if (!_spawnPoints.Contains(spawnPoint)) _spawnPoints.Add(spawnPoint);
    }

    // Unregister an existing spawn point
    public void UnRegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPoints.Remove(spawnPoint);
    }

    // Regularly clean up null spawn points
    void Update()
    {
        _spawnPoints.RemoveAll(s => s == null);
    }

    // Method to spawn a wave (not directly used by SpawnPoint anymore)
    public void SpawnWave()
    {
        foreach (SpawnPoint point in _spawnPoints)
        {
            point.Spawn();
        }
    }
}

