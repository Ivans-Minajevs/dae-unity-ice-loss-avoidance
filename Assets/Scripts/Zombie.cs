using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : BasicCharacter
{
    private Animator _animator;
    private GameObject _playerTarget;
    [SerializeField] private GameObject _attackVFXTemplate = null;
    private bool _isAttacking = false;

    [SerializeField] private GameObject audioSourcePrefab;  
    [SerializeField] private UnityEvent _onDeathEvent;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        Father player = FindObjectOfType<Father>();  // Assuming Father is the player class
        if (player) 
        {
            _playerTarget = player.gameObject;
            _movementBehaviour.IsMoving = true;
        }
    }
    private const float stopThreshold = 1.5f;
    private void Update()
    {
        if (_playerTarget == null) return;
        
        if (_isAttacking)
        {
            AnimatorStateInfo attackStateInfo = _animator.GetCurrentAnimatorStateInfo(1);  
            if (attackStateInfo.IsName("Z_Attack") && attackStateInfo.normalizedTime >= 1f)
            {
                _isAttacking = false;
            }
            else
            {
                _movementBehaviour.IsMoving = false;
            }
        }
        else
        {
            _movementBehaviour.IsMoving = true;
            Vector3 horizontalDifference = _playerTarget.transform.position - transform.position;
            horizontalDifference.y = 0; 
            if (horizontalDifference.magnitude < stopThreshold)
            {
                _isAttacking = true;
                _movementBehaviour.IsMoving = false;
            }
        }
        HandleAttackAnimation();
        
        if (_movementBehaviour.IsMoving)
        {
            LookAtPlayer();
            HandleMovementAnimation();
            HandleMovement();
        }
        
    }
    private const string IS_ATTACKING_PARAM = "IsAttacking";
    void HandleAttackAnimation()
    {
        if (_animator == null) return;
        _animator.SetBool(IS_ATTACKING_PARAM, _isAttacking); // Update attacking layer
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
        direction.y = 0; 
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void HandleMovement()
    {
        if (_movementBehaviour == null) return;
        _movementBehaviour.Target = _playerTarget;
        _movementBehaviour.IsMoving = true;
    }

    public void Kill()
    { 
        if (_attackVFXTemplate)
            Instantiate(_attackVFXTemplate, transform.position, transform.rotation);
        
        if (audioSourcePrefab)
        {
            GameObject audioSourceInstance = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            AudioSource audio = audioSourceInstance.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
                Destroy(audioSourceInstance, audio.clip.length); 
            }
        }
        _onDeathEvent?.Invoke();
        Destroy(gameObject);  
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Friendly")) 
        {
            Father father = collision.gameObject.GetComponent<Father>();
            if (father != null && _isAttacking)
            {
                father.DecreaseHealth();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword")) //If collide with Sword
        {
            Kill();  
        }
    }
}
