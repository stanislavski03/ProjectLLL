using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private GameObject _gameCamera;
    [SerializeField] private GameObject _menuCamera;

    public Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;

        _gameCamera.SetActive(!_gameCamera.activeSelf);
        _menuCamera.SetActive(!_gameCamera.activeSelf);
    }
}
