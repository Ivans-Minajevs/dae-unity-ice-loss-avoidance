using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HUD : MonoBehaviour
{

    [SerializeField] Volume _globalVolume;
    
    [SerializeField] protected StyleSheet sheet;
    
    private UIDocument _attachedDocument;
    private VisualElement _root;

    private ProgressBar _healthBar;
    private ProgressBar _frostbiteBar;

    private Label _metalLabel;
    private Label _woodLabel;
    private Label _plasticLabel;

    void Start()
    {
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument)
        {
            _root = _attachedDocument.rootVisualElement;
            
        }

        if (_root != null)
        {
            _healthBar = _root.Q<ProgressBar>("FatherHealthBar");
            _frostbiteBar = _root.Q<ProgressBar>("FatherFrostbiteBar");
            
           _root.styleSheets.Add(sheet);
           _root.MarkDirtyRepaint();
           //
           //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/HUD/test.uss");
           //_root.styleSheets.Add(styleSheet);
           

            _metalLabel = _root.Q<Label>("MetalLabel");
            _woodLabel = _root.Q<Label>("WoodLabel");
            _plasticLabel = _root.Q<Label>("PlasticLabel");


            Father father = FindObjectOfType<Father>();
            if (father != null)
            {
                Health fatherHealth = father.GetComponent<Health>();
                if (fatherHealth)
                {
                    UpdateHealth(fatherHealth.StartHealth, fatherHealth.CurrentHealth);

                    fatherHealth.OnHealthChanged += UpdateHealth;
                }

                Frostbite fatherFrostbite = father.GetComponent<Frostbite>();
                if (fatherFrostbite)
                {
                    UpdateFrostbite(fatherFrostbite.MaxFrostbite, fatherFrostbite.CurrentFrostbite);

                    fatherFrostbite.OnFrostbiteChanged += UpdateFrostbite;
                }

                FatherInventory inventory = father.GetComponent<FatherInventory>();
                if (inventory)
                {
                    UpdateMetal((inventory.CurrentMetal));
                    UpdateWood((inventory.CurrentWood));
                    UpdatePlastic((inventory.CurrentPlastic));

                    inventory.OnMetalItemChanged += UpdateMetal;
                    inventory.OnWoodItemChanged += UpdateWood;
                    inventory.OnPlasticItemChanged += UpdatePlastic;
                }

            }
        }
    }

    public void UpdateMetal(int amount)
    {
        _metalLabel.text = string.Format("Metal: {0}", amount);
    }
    
    public void UpdateWood(int amount)
    {
        _woodLabel.text = string.Format("Wood: {0}", amount);
    }
    
    public void UpdatePlastic(int amount)
    {
        _plasticLabel.text = string.Format("Plastic: {0}", amount);
    }


    public void UpdateHealth(float startHealth, float currentHealth)
    {
        if (_healthBar == null) return;

        _healthBar.value = currentHealth / startHealth * 100;
        _healthBar.title = string.Format("{0}/{1}", currentHealth, startHealth);
    }

    public void UpdateFrostbite(float maxFrostbite, float currentFrostbite)
    {
        if (_frostbiteBar == null) return;
        
        //vignette.intensity.value = currentFrostbite/maxFrostbite;
        if (_globalVolume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = currentFrostbite / maxFrostbite;
        }

        _frostbiteBar.value = currentFrostbite / maxFrostbite * 100;
        _frostbiteBar.title = string.Format("{0}/{1}", currentFrostbite, maxFrostbite);
    }
}
