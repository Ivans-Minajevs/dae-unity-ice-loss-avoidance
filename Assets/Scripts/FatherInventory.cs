using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherInventory : MonoBehaviour
{
    public int metalCount = 0;
    public int plasticCount = 0;
    public int woodCount = 0;

    public void AddMaterial(Collectible.MaterialType materialType)
    {
        switch (materialType)
        {
            case Collectible.MaterialType.Metal:
                metalCount++;
                break;
            case Collectible.MaterialType.Plastic:
                plasticCount++;
                break;
            case Collectible.MaterialType.Wood:
                woodCount++;
                break;
        }
        
        // Optional: Trigger HUD update or display collected item feedback
        Debug.Log($"Collected {materialType}. Metal: {metalCount}, Plastic: {plasticCount}, Wood: {woodCount}");
    }
}
