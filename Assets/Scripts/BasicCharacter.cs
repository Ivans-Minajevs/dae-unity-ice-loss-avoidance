using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    protected AttackBehaviour _attackBehaviour;

    protected MovementBehaviour _movementBehaviour;
    


  
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _movementBehaviour = GetComponent<MovementBehaviour>();
    }

}
