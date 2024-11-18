using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherInventory : MonoBehaviour
{
    public int metalCount = 0;
    public int plasticCount = 0;
    public int woodCount = 0;

    public delegate void MetalItemChange(int metalCount);
    public event MetalItemChange OnMetalItemChanged;
    
    public delegate void PlasticItemChange(int plasticCount);
    public event PlasticItemChange OnPlasticItemChanged;
    
    public delegate void WoodItemChange(int woodCount);
    public event WoodItemChange OnWoodItemChanged;
    
    public int CurrentMetal { get { return metalCount; } }
    public int CurrentPlastic { get { return plasticCount; } }
    public int CurrentWood { get { return woodCount; } }
    public void AddMaterial(Collectible.MaterialType materialType)
    {
        switch (materialType)
        {
            case Collectible.MaterialType.Metal:
                metalCount++;
                OnMetalItemChanged?.Invoke(metalCount);
                break;
            case Collectible.MaterialType.Plastic:
                plasticCount++;
                OnPlasticItemChanged?.Invoke(plasticCount);
                break;
            case Collectible.MaterialType.Wood:
                woodCount++;
                OnWoodItemChanged?.Invoke(woodCount);
                break;
        }
    }
    
    public bool SpendResources(int requiredMetal, int requiredPlastic, int requiredWood)
    {
        if (metalCount >= requiredMetal && plasticCount >= requiredPlastic && woodCount >= requiredWood)
        {
            metalCount -= requiredMetal;
            OnMetalItemChanged?.Invoke(metalCount);
            
            plasticCount -= requiredPlastic;
            OnPlasticItemChanged?.Invoke(plasticCount);
            
            woodCount -= requiredWood;
            OnWoodItemChanged?.Invoke(woodCount);
            return true;
        }
        return false;
    }
}
