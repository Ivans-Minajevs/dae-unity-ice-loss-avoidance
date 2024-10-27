using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] private Material bonfireMaterial;
    private Color inactiveColor = new Color(0.65f, 0.33f, 0.16f);
    private Color activeColor = new Color(1f, 0.5f, 0.1f);

    private bool _isActivated = false;

    public bool IsActive()
    {
        return _isActivated;
    }
    private void Start()
    {
        bonfireMaterial.color = inactiveColor;
    }

    public void ActivateBonfire()
    {
        bonfireMaterial.color = activeColor;
        _isActivated = true;
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

