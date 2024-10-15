using System.Collections;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _swordTemplate;
    [SerializeField] private GameObject _handBone;

    private GameObject _sword; 
    private bool _isAttacking = false; 

    public bool IsAttacking
    {
        get => _isAttacking;
        set => _isAttacking = value;
    }

    void Awake()
    {
        if (_swordTemplate != null && _handBone != null)
        {
            _sword = Instantiate(_swordTemplate, _handBone.transform, true);
            _sword.transform.localPosition = Vector3.zero;
            _sword.transform.localRotation = Quaternion.identity;
            _sword.SetActive(false); 
        }
    }
    
    public void Attack()
    {
        if (_sword != null && !_isAttacking)
        {
            _isAttacking = true;  
            _sword.SetActive(true); 
        }
    }

    public void EndAttack()
    {
        _isAttacking = false; 

        if (_sword != null)
        {
            _sword.SetActive(false); 
        }
    }
}

