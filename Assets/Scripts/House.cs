using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private Material houseMaterial;
    private Color inactiveColor = new Color(0.65f, 0.33f, 0.16f);
    private Color activeColor = new Color(1f, 0.5f, 0.1f);
    
    private void Start()
    {
        houseMaterial.color = inactiveColor;
    }
    private const string PLAYER_TAG = "Friendly";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Father father = other.GetComponent<Father>();
            if (father != null)
            {
                father.SetHouse(this);
                houseMaterial.color = activeColor;
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
                father.ClearHouse();
                houseMaterial.color = inactiveColor;
            }
        }
    }
}


