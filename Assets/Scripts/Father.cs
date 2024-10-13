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
    
    protected Animator _animator;
    
    private void Start()
    {
        _animator = transform.GetComponent<Animator>();
    }
    

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
            StartCoroutine(_attackBehaviour.EndAttack(0.0f));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move normally to clicked position
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                _movementBehaviour.DesiredMovementDirection = (targetPosition - transform.position).normalized;
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
    
    private const string IS_ATTACKING_PARAM = "IsAttacking";
    void HandleAttackAnimation() 
    {
        if (_animator == null) return;
        
        _animator.SetTrigger(IS_ATTACKING_PARAM);
    }

    void TriggerAttack()
    {
        // Call the AttackBehaviour to handle sword movement
        if (_attackBehaviour != null && !_attackBehaviour.IsAttacking) 
        {
            
            HandleAttackAnimation();
            _attackBehaviour.Attack();  // This will handle the sword movement during the attack
            _movementBehaviour.IsClosedToEnemy = false;
        }
    }
    
   private void HandleAttackInput()
    {
        if (_autoAttack == null)
        { 
            return;
        }
        if (_autoAttack.action.IsPressed())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy")) 
                {
                    _movementBehaviour.Target = hit.collider.gameObject; // Set the enemy as the target
                    Vector3 targetPosition = hit.point;

                    // Ensure the character only moves on the X and Z axis
                    targetPosition.y = transform.position.y;

                    // Set end position and start moving toward enemy
                    _movementBehaviour.DesiredMovementDirection = (targetPosition - transform.position).normalized;
                    _movementBehaviour.IsMoving = true;
                    _movementBehaviour.EndPosition = targetPosition;
                }
            }
        }
    }
  

    // Update is called once per frame
    void Update()
    {
       
        HandleAttackInput();
        if (_movementBehaviour.IsClosedToEnemy == true)
        {
            TriggerAttack();
        }
        
        HandleMovementInput();
        HandleMovementAnimation();
        
        
    }
}
