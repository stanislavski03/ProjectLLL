using System.Collections;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private GameObject _gameCamera;
    [SerializeField] private GameObject _menuCamera;
    [SerializeField] private float _cameraSwitchDelay = 0.5f;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Gameplay)
        {
            StartCoroutine(SwitchToGameCamera());
        }
        else if (newGameState == GameState.Paused)
        {
            SwitchToMenuCameraImmediately();
        }
        // LevelUpPaused - камера не меняется
    }

    private IEnumerator SwitchToGameCamera()
    {
        yield return new WaitForSeconds(_cameraSwitchDelay);
        
        _gameCamera.SetActive(true);
        _menuCamera.SetActive(false);
    }

    private void SwitchToMenuCameraImmediately()
    {
        _menuCamera.SetActive(true);
        _gameCamera.SetActive(false);
    }
}