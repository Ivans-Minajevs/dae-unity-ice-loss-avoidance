using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    [SerializeField] private GameObject _rightArm;
    [SerializeField] private Material _heartMat;     
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    void Start()
    {
        // Hide the arm and heart at the beginning
        HideRightArm();
        HideHeart();
    }

    void HideRightArm()
    {
        if (_rightArm != null)
        {
            _rightArm.transform.localScale = Vector3.zero;  // Disable the entire arm GameObject
        }
    }

    void HideHeart()
    {
        if (_heartMat != null)
        {
            _heartMat.color = Color.red;
        }
    }
    public void EnablePart(string partName)
    {
        switch (partName)
        {
            case "Arm":
                _rightArm.transform.localScale = Vector3.one;
                break;
            case "Heart":
                _heartMat.color = Color.green;
                break;
        }
    }
}
