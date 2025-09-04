using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private float _speed;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _damageCooldown = 1f;
    

    private Transform _playerTransform;
    private float _cooldownTimer = 0;

    private void OnEnable()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        LookAt();
        GoFoward();
    }

    private void OnCollisionStay(Collision collision)
    {
        PlayerCheckAndDamage(collision);
    }

    private void LookAt()
    {
        Vector3 _lookTarget = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);
        transform.LookAt(_lookTarget);
    }
    private void GoFoward()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) > 1)
            transform.position += transform.forward * _speed * Time.fixedDeltaTime;
    }


    private void PlayerCheckAndDamage(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHP player) && _cooldownTimer <= 0)
        {
            player.Damage(_damage);
            StartCoroutine(DamageCooldown());
        }
    }
    private IEnumerator DamageCooldown()
    {
        for(_cooldownTimer = _damageCooldown; _cooldownTimer > 0; _cooldownTimer -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }

    }
}
