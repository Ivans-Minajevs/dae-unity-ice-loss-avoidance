using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : BasicCharacter
{
    private Animator _animator;
    private GameObject _playerTarget;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Father player = FindObjectOfType<Father>();  // Assuming Father is the player class
        if (player) 
        {
            _playerTarget = player.gameObject;
            _attackBehaviour = player.GetComponent<AttackBehaviour>();  // Get the player's attack behaviour
        }
    }

    private void Update()
    {
        if (_playerTarget == null) return;

        // Make the zombie look at the player
        LookAtPlayer();

        // Move toward the player
        HandleMovement();
        
        HandleMovementAnimation();
    }

    private const string IS_MOVING_PARAM = "IsMoving";
    void HandleMovementAnimation() 
    {
        if (_animator == null) return;
        
        _animator.SetBool(IS_MOVING_PARAM, _movementBehaviour.IsMoving);
    }
    private void LookAtPlayer()
    {
        Vector3 direction = (_playerTarget.transform.position - transform.position).normalized;
        direction.y = 0;  // Ensure the zombie doesn't tilt upwards or downwards
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void HandleMovement()
    {
        if (_movementBehaviour == null) return;
        
        _movementBehaviour.Target = _playerTarget;
    }

    private void Kill()
    {
        Destroy(gameObject);  // Destroy the zombie when killed
    }

    // Handle sword collision
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player's sword and the player is currently attacking
        if (other.CompareTag("Sword") && _attackBehaviour.IsAttacking)
        {
            Kill();  // Destroy zombie if hit by sword during an attack
        }
    }
}
