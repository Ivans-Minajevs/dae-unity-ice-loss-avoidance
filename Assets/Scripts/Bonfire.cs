using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] private Material bonfireMaterial;
    private Color inactiveColor = new Color(0.65f, 0.33f, 0.16f);
    private Color activeColor = new Color(1f, 0.5f, 0.1f);

    private bool _isActivated = false;
    private Renderer _bonfireRenderer;
    private Material _currentMaterial;

    public bool IsActive()
    {
        return _isActivated;
    }

    private void Start()
    {
        _bonfireRenderer = GetComponent<Renderer>();

        // Ensure the bonfire has a unique material instance
        if (bonfireMaterial != null && _bonfireRenderer != null)
        {
            // If there's an old material, clean it up first
            if (_currentMaterial != null)
            {
                Destroy(_currentMaterial); // Clean up the old material
            }

            // Create a new material instance
            _currentMaterial = new Material(bonfireMaterial);
            _bonfireRenderer.material = _currentMaterial; // Apply the new material
            _currentMaterial.color = inactiveColor; // Set the initial color
        }
    }

    public void ActivateBonfire()
    {
        if (_bonfireRenderer != null && _currentMaterial != null)
        {
            _currentMaterial.color = activeColor; // Change color when activated
            _isActivated = true;
        }
    }

    private const string PLAYER_TAG = "Friendly";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Father father = other.GetComponent<Father>();
            if (father != null)
            {
                father.SetCurrentBonfire(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Father father = other.GetComponent<Father>();
            if (father != null)
            {
                father.ClearCurrentBonfire();
            }
        }
    }
}
