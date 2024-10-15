using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum MaterialType { Metal, Plastic, Wood }
    public MaterialType materialType;

    private const string PLAYER_TAG = "Friendly";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Father father = other.GetComponent<Father>();
            if (father != null)
            {
                father.SetCurrentCollectible(this);
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
                father.ClearCurrentCollectible();
            }
        }
    }
}

