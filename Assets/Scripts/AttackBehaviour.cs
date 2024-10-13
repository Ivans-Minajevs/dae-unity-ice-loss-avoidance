using System.Collections;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _swordTemplate;
    [SerializeField] private GameObject _handBone;  // The bone where the sword should be attached

    private GameObject _sword;  // The instantiated sword
    private bool _isAttacking = false;  // Flag to track if the character is in an attack animation

    public bool IsAttacking => _isAttacking;

    void Awake()
    {
        // Instantiate the sword and attach it to the hand (or wherever you want)
        if (_swordTemplate != null && _handBone != null)
        {
            _sword = Instantiate(_swordTemplate, _handBone.transform, true);
            _sword.transform.localPosition = Vector3.zero;
            _sword.transform.localRotation = Quaternion.identity;
            _sword.SetActive(false);  // Initially hide the sword
        }
    }

    // Method to handle sword animation during attack
    public void Attack()
    {
        if (_sword != null && !_isAttacking)
        {
            _isAttacking = true;  

            _sword.SetActive(true);  

            // You can add any other logic here to handle hit detection or other attack mechanics
            StartCoroutine(EndAttack(1.5f)); 
        }
    }

    // Coroutine to reset attack state after animation is finished
    public IEnumerator EndAttack(float duration)
    {
        yield return new WaitForSeconds(duration);  // Wait for the attack animation to finish
        _isAttacking = false;  // Reset the attack state

        if (_sword != null)
        {
            _sword.SetActive(false);  // Hide the sword after the attack
        }
    }
}

