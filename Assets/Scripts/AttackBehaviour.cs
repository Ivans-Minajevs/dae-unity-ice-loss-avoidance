using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _gunTemplate;

    [SerializeField] private GameObject _socket;

    //private BasicWeapon _weapon;

    void Awake()
    {
        if (_gunTemplate != null && _socket != null)
        {
            GameObject gunObject = Instantiate(_gunTemplate, _socket.transform, true);
            gunObject.transform.localPosition = Vector3.zero;
            gunObject.transform.localRotation = Quaternion.identity;
            //_weapon = gunObject.GetComponent<BasicWeapon>();
        }
    }
    
    public void Attack()
    {
        
    }
}

