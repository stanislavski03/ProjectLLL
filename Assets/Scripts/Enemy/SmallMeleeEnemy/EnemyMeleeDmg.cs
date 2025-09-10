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
        CountdownController.OnCountdownStarted += OnCountdownStarted;
        CountdownController.OnCountdownFinished += OnCountdownFinished;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        CountdownController.OnCountdownStarted -= OnCountdownStarted;
        CountdownController.OnCountdownFinished -= OnCountdownFinished;
    }

    private void OnCollisionStay(Collision collision)
    {
        PlayerCheckAndDamage(collision);
    }

    private void PlayerCheckAndDamage(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHP player) && _cooldownTimer <= 0 && enabled)
        {
            player.Damage(_damage);
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

    public void ReturnToPool()
    {
        EnemyPool.Instance.GetEnemyBackToPool(gameObject);
    }

    private void OnCountdownStarted()
    {
        enabled = false;
    }

    private void OnCountdownFinished()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            enabled = true;
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Paused || newGameState == GameState.LevelUpPaused)
        {
            enabled = false;
        }
        else if (newGameState == GameState.Gameplay)
        {
            enabled = false;
        }
    }
}
