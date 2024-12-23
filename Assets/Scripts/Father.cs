using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Father : BasicCharacter
{
    
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private InputActionReference _autoAttack;
    [SerializeField] private InputActionReference _ability1;
    [SerializeField] private InputActionReference _ability2;
    [SerializeField] private InputActionReference _interact;
    
    private DialogueManager _dialogueManager;
    [SerializeField] private Mechanism _mechanism;
    [SerializeField] private GameObject _attackVFXTemplate = null;
    private Bonfire _currentBonfire;
    private House _house;
    
    protected Animator _animator;
    private FatherInventory _inventory;

    private Frostbite _frostbite;
    private Health _health;

    private static readonly string IS_MOVING_PARAM = "IsMoving";
    private static readonly string IS_ATTACKING_PARAM = "IsAttacking";
    private static readonly string IS_COLLECTING_PARAM = "IsCollecting";

    protected bool _isAttackActivated = false;
    private Collectible _currentCollectible;
    private Coroutine _frostReductionCoroutine;
    private Coroutine _healthIncreaseCoroutine;

    private void Start()
    {
        _animator = transform.GetComponent<Animator>();
        _inventory = GetComponent<FatherInventory>();
        _frostbite = GetComponent<Frostbite>();
        _health = GetComponent<Health>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        InvokeRepeating("IncreaseFrostbiteValue", 35f, 2f);
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
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 2000, int.MaxValue, QueryTriggerInteraction.Ignore))
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
    }

    void HandleMovementAnimation()
    {
        if (_animator == null) return;
        _animator.SetBool(IS_MOVING_PARAM, _movementBehaviour.IsMoving); // Update base layer for movement
    }

    void HandleAttackAnimation()
    {
        if (_animator == null) return;
        _animator.SetBool(IS_ATTACKING_PARAM, _attackBehaviour.IsAttacking); // Update attacking layer
    }

    void HandleAttackInput()
    {
        if (_autoAttack == null)
        {
            return;
        }

        if (_autoAttack.action.IsPressed())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2000, int.MaxValue, QueryTriggerInteraction.Ignore))
            {
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
        if (_interact.action.IsPressed() && Vector3.Distance(transform.position, _mechanism.GetPosition()) < 1.0f)
        {
            _dialogueManager.ShowDialogue();
        }

        if (_interact.action.IsPressed() && _currentBonfire != null && _currentBonfire.IsActive() == false)
        {
            if (TryActivateBonfire(1, 3))
            {
                _frostReductionCoroutine = StartCoroutine(DecreaseFrostOverTime());
            }
        }
    }

    private void CollectMaterial()
    {
        if (_currentCollectible != null)
        {
            _animator.SetBool(IS_COLLECTING_PARAM, true);  // Set collecting animation on Collecting Layer

            _inventory.AddMaterial(_currentCollectible.materialType);  // Add material to inventory
            Destroy(_currentCollectible.gameObject);  // Destroy the collectible after collecting it

            ClearCurrentCollectible();
        }
    }
    
    
    public bool TryBuildRobotPart(string part, int requiredMetal, int requiredPlastic, int requiredWood)
    {
        if (_inventory.SpendResources(requiredMetal, requiredPlastic, requiredWood))
        {
            _mechanism.EnablePart(part);
            return true;
        }
        else
        {
            Debug.Log("Not enough resources to build the " + part);
            return false;
        }
    }

    private bool TryActivateBonfire(int requiredPlastic, int requiredWood)
    {
        if (_inventory.SpendResources(0, requiredPlastic, requiredWood))
        {
            _currentBonfire.ActivateBonfire();
            return true;
        }
        else
        {
            Debug.Log("Not enough resources to build the bonfire");
            return false;
        }
    }

    void IncreaseFrostbiteValue()
    {
        _frostbite.Freeze(5);
    }

    public void DecreaseHealth()
    {
        _health.Damage(1);
    }
    // Update is called once per frame
    void Update()
    {
        // Check if the character is in the Collecting state on Collecting Layer (layer 2)
        AnimatorStateInfo collectingStateInfo = _animator.GetCurrentAnimatorStateInfo(2);
        bool isCollecting = collectingStateInfo.IsName("Gathering") && _animator.GetBool(IS_COLLECTING_PARAM);

        
        if (isCollecting)
        {
            _movementBehaviour.IsMoving = false;
            if (collectingStateInfo.normalizedTime >= 0.8f)
            {
                _animator.SetBool(IS_COLLECTING_PARAM, false);  // Reset the collecting parameter after animation finishes
            }
            return;
        }
        
        
        HandleAttackInput();
        if (_attackBehaviour.IsAttacking)
        {
            AnimatorStateInfo attackStateInfo = _animator.GetCurrentAnimatorStateInfo(1);  // Attacking layer
            if (attackStateInfo.IsName("Attack") && attackStateInfo.normalizedTime >= 0.8f)
            {
                _attackBehaviour.EndAttack();
            }
            else
            {
                _movementBehaviour.IsMoving = false;
            }
        }
        else
        {
            if (_movementBehaviour.IsClosedToEnemy && _isAttackActivated)
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

    public void SetCurrentBonfire(Bonfire bonfire)
    {
        _currentBonfire = bonfire;

        if (_currentBonfire.IsActive())
        {
            _frostReductionCoroutine = StartCoroutine(DecreaseFrostOverTime());
        }
    }

    public void SetHouse(House house)
    {
        _house = house;
       
        if (_house.IsActive())
        {
            _frostReductionCoroutine = StartCoroutine(DecreaseFrostOverTime());
            _healthIncreaseCoroutine = StartCoroutine(IncreaseHealthOverTime());
        }
        
    }
    public IEnumerator DecreaseFrostOverTime()
    {
        while ((_house != null && _house.IsActive()) || (_currentBonfire != null && _currentBonfire.IsActive()))
        {
            _frostbite.Heat(10);  // Decrease frost value
            yield return new WaitForSeconds(1f); 
        }
    }

    private IEnumerator IncreaseHealthOverTime()
    {
        while (_house != null)
        {
            _health.Heal(1);
            yield return new WaitForSeconds(1f); 
        }
    }

    public void ClearHouse()
    {
        _house = null;
        if (_frostReductionCoroutine != null)
        {
            StopCoroutine(_frostReductionCoroutine);
            _frostReductionCoroutine = null;
        }
    }

    public void ClearCurrentBonfire()
    {
        _currentBonfire = null;
        
        if (_frostReductionCoroutine != null)
        {
            StopCoroutine(_frostReductionCoroutine);
            _frostReductionCoroutine = null;
        }
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
    
    public void Kill()
    { 
        if (_attackVFXTemplate)
            Instantiate(_attackVFXTemplate, transform.position, transform.rotation);
        Destroy(gameObject);  
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}