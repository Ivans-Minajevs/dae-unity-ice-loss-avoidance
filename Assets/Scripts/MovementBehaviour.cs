using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class MovementBehaviour : MonoBehaviour
{
   [SerializeField] protected float _movementSpeed = 1.0f;

   protected bool _isMoving = false;
   protected Rigidbody _rigidBody;
   
   protected Vector3 _desiredMovementDirection = Vector3.zero;
   protected Vector3 _endPosition = Vector3.zero;

   protected GameObject _target;
   
   protected bool _grounded = false;

   protected const float GROUND_CHECK_DISTANCE = 0.01f;
   protected const string GROUND_LAYER = "Ground";
   
   protected const float _attackRange = 2.0f;
   protected bool _isClosedToEnemy = false;
   public Vector3 DesiredMovementDirection
   {
      get { return _desiredMovementDirection; }
      set { _desiredMovementDirection = value; }
   }

   public bool IsClosedToEnemy
   {
      get { return _isClosedToEnemy;  }
      set { _isClosedToEnemy = value;  }
   }
   public bool IsMoving
   {
      get { return _isMoving; }
      set { _isMoving = value; }
   }
   
   public Vector3 EndPosition
   {
      get { return _endPosition; }
      set { _endPosition = value; }
   }
   public GameObject Target
   {
      get { return _target; }
      set { _target = value;  }
   }
  
   protected virtual void Awake()
   {
      _rigidBody = GetComponent<Rigidbody>();
   }

   
   protected virtual void FixedUpdate()
   {
      
      HandleStopping();
      HandleMovement();
      
      _grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down,
         GROUND_CHECK_DISTANCE, LayerMask.GetMask(GROUND_LAYER));
   }
   
   protected virtual void Update()
   {
      CheckDistanceToEnemy();
   }

   private void CheckDistanceToEnemy()
   {
      if (_target != null)
      {
         // If close to the enemy, stop moving and attack
         float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);
         if (distanceToTarget <= _attackRange)
         {
            _isMoving = false; // Stop moving
            transform.LookAt(_target.transform.position); // Face the enemy
            _target = null; // Clear target after attacking (optional, depending on your logic)
            _isClosedToEnemy = true; // Trigger the attack
         }
      }
   }

   protected virtual void HandleMovement()
   {
      if (_rigidBody == null) return;

      Vector3 movement = default;
      
      if (_isMoving)
      {
         movement = _desiredMovementDirection;
         
         movement *= _movementSpeed;
         
         transform.LookAt(_endPosition);
      }
      
      movement.y = _rigidBody.velocity.y; //if we dont do that it will cancel out gravity     
      _rigidBody.velocity = movement;
   }

   private const float stopThreshold = 0.1f;
   private void HandleStopping()
   {
      if (_isMoving)
      {
         // Check if the character is not already at the EndPosition
         if (Vector3.Distance(transform.position, _endPosition) < stopThreshold)
         {
            _isMoving = false;
            transform.position = _endPosition;
         }
      }
   }
}
