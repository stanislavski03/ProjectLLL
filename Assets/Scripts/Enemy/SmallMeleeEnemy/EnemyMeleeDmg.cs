using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDmg : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _damageCooldown = 1f;

    private float _cooldownTimer = 0;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnCollisionStay(Collision collision)
    {
        PlayerCheckAndDamage(collision);

    }

    private void PlayerCheckAndDamage(Collision collision)
    {
        if (collision.gameObject.GetComponent< PlayerHP>() && _cooldownTimer <= 0)
        {
            collision.gameObject.GetComponent<PlayerHP>().Damage(_damage);
            StartCoroutine(DamageCooldown());
        }
    }
    private IEnumerator DamageCooldown()
    {
        for (_cooldownTimer = _damageCooldown; _cooldownTimer > 0; _cooldownTimer -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }

    }

    private void OnGameStateChanged(GameState newGameState)
    {
        if (gameObject.activeSelf)
        {
            enabled = newGameState == GameState.Gameplay;
            //ситуативно, если надо чтобы враги не наносили урон сразу как отожмётся пауза

            if (enabled)
            {
                StartCoroutine(DamageCooldown());
            }
            else
            {
                StopCoroutine(DamageCooldown());
            }
        }
    }

    public void ReturnToPool()
    {
        EnemyPool.Instance.GetEnemyBackToPool(gameObject);
    }
}
