using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    protected AttackBehaviour _attackBehaviour;

    protected MovementBehaviour _movementBehaviour;

    protected Animator _animator;
  
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _movementBehaviour = GetComponent<MovementBehaviour>();
    }

}
