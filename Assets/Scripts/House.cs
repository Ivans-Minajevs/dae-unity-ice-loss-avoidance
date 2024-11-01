using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private Material houseMaterial;
    private Color _inactiveColor = new Color(0.65f, 0.33f, 0.16f);
    private Color _activeColor = new Color(1f, 0.5f, 0.1f);

    private bool _isActivated = false;

    public bool IsActive()
    {
        return _isActivated;
    }
    private void Start()
    {
        houseMaterial.color = _inactiveColor;
    }

    public void ActivateHouse()
    {
        _isActivated = true;
        houseMaterial.color = _activeColor;
    }

    public void DeactivateHouse()
    {
        _isActivated = false;
        houseMaterial.color = _inactiveColor;
    }
    
    private const string PLAYER_TAG = "Friendly";
    private const string ENEMY_TAG = "Enemy";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Father father = other.GetComponent<Father>();
            if (father != null)
            {
                ActivateHouse();
                father.SetHouse(this);
            }
        }

        if (other.CompareTag(ENEMY_TAG))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            zombie.Kill();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Father father = other.GetComponent<Father>();
            if (father != null)
            {
                DeactivateHouse();
                father.ClearHouse();
            }
        }
    }
}


