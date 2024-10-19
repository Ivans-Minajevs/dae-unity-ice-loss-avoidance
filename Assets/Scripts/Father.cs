using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private FatherInventory _inventory;

    private Frostbite _frostbite;
    private Energy _energy;

    private static readonly string IS_MOVING_PARAM = "IsMoving";
    private static readonly string IS_ATTACKING_PARAM = "IsAttacking";
    private static readonly string IS_COLLECTING_PARAM = "IsCollecting";
    
    protected bool _isAttackActivated = false;
    private Collectible _currentCollectible;
    
    private void Start()
    {
        _animator = transform.GetComponent<Animator>();
        _inventory = GetComponent<FatherInventory>();
        _frostbite = GetComponent<Frostbite>();
        _energy = GetComponent<Energy>();
        InvokeRepeating("IncreaseFrostbiteValue", 5f, 2f);
        InvokeRepeating("DecreaseEnergyValue", 5f, 2f);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            
            if (Physics.Raycast(ray, out hit))
            {
                _attackBehaviour.EndAttack();
                _isAttackActivated = false;
                // Move normally to clicked position
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                _movementBehaviour.DesiredMovementDirection = (targetPosition - transform.position).normalized;
                _movementBehaviour.IsMoving = true;
                _movementBehaviour.EndPosition = targetPosition;
                
            }
        }
    }
    


    void HandleMovementAnimation() 
    {
        if (_animator == null) return;
        _animator.SetBool(IS_MOVING_PARAM, _movementBehaviour.IsMoving);
    }
    
    void HandleAttackAnimation() 
    {
        if (_animator == null) return;
        _animator.SetBool(IS_ATTACKING_PARAM, _attackBehaviour.IsAttacking);
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
                //Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.red);
                
                if (hit.collider.CompareTag("Enemy") && !_attackBehaviour.IsAttacking) 
                {
                    _movementBehaviour.Target = hit.collider.gameObject; // Set the enemy as the target
                    Vector3 targetPosition = hit.point;

                    // Ensure the character only moves on the X and Z axis
                    targetPosition.y = transform.position.y;

                    // Set end position and start moving toward enemy
                    _movementBehaviour.DesiredMovementDirection = (targetPosition - transform.position).normalized;
                    _movementBehaviour.IsMoving = true;
                    _movementBehaviour.EndPosition = targetPosition;
                    
                    _isAttackActivated = true;
                }
            }
        }
    }
  
    void HandleCollectingInput()
    {
        if (_interact == null) return;

        if (_interact.action.IsPressed() && _currentCollectible != null)
        {
            CollectMaterial();
        }
    }
    
    private void CollectMaterial()
    {
        if (_currentCollectible != null)
        {
            _animator.SetTrigger(IS_COLLECTING_PARAM);  // Play collecting animation

            _inventory.AddMaterial(_currentCollectible.materialType);  // Add material to inventory
            Destroy(_currentCollectible.gameObject);  // Destroy the collectible after collecting it

            ClearCurrentCollectible(); 
        }
    }

    void IncreaseFrostbiteValue()
    {
        _frostbite.Freeze(1);
    }
    
    void DecreaseEnergyValue()
    {
        _energy.Tire(1);
    }
    // Update is called once per frame
    void Update()
    {
        
        HandleAttackInput();
        if (_attackBehaviour.IsAttacking)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") )// If the animation is finished
            {
                if (stateInfo.normalizedTime >= 1.0f)
                {
                    _attackBehaviour.EndAttack();
                }
                else
                {
                    _movementBehaviour.IsMoving = false;
                }
            }
        }
        else
        {
            if (_movementBehaviour.IsClosedToEnemy && _isAttackActivated )
            {
                if (_attackBehaviour != null) 
                {
                    _attackBehaviour.Attack();  // This will handle the sword movement during the attack
                    _isAttackActivated = false;
                    
                }
            }
        }
        HandleAttackAnimation();
        
        HandleMovementInput();
        HandleMovementAnimation();
        
        HandleCollectingInput();  
    }
    
    public void SetCurrentCollectible(Collectible collectible)
    {
        _currentCollectible = collectible;
    }

    // Call when the player exits collectible range
    public void ClearCurrentCollectible()
    {
        _currentCollectible = null;
    }
}

