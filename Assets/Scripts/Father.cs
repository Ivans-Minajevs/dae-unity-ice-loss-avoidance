using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Father : BasicCharacter
{
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private InputActionReference _autoAttack;
    [SerializeField] private InputActionReference _ability1;
    [SerializeField] private InputActionReference _ability2;
    [SerializeField] private InputActionReference _interact;
    
    private void OnEnable()
    {
        if (_inputAsset == null) return;
        
        _inputAsset.Enable();
    }
    
    private void OnDisable()
    {
        if (_inputAsset == null) return;
        
        _inputAsset.Disable();
    }

    void HandleMovementInput()
    {
        if (_movementBehaviour == null || _movementAction == null)
        { 
            return;
        }
        
        if (_movementAction.action.IsPressed())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // The point where the ray hits the plane
                Vector3 targetPosition = hit.point;

                // Ensure the character only moves on the X and Z axis
                targetPosition.y = transform.position.y;

                // Calculate the direction from the character to the target position
                _movementBehaviour.DesiredMovementDirection = (targetPosition - transform.position).normalized;

                // Set end position and start moving
                _movementBehaviour.IsMoving = true;
                _movementBehaviour.EndPosition = targetPosition;
            }
            
        }
    }
    

    private const string IS_MOVING_PARAM = "IsMoving";
    void HandleMovementAnimation() 
    {
        if (_animator == null) return;
        
        _animator.SetBool(IS_MOVING_PARAM, _movementBehaviour.IsMoving);
    }

   /* private void HandleAttackInput()
    {
        if (_attackBehaviour == null || _shootAction == null) return;

        if (_shootAction.IsPressed()) _attackBehaviour.Attack();
    }*/
  

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMovementAnimation();
        //HandleAttackInput();
    }
}
