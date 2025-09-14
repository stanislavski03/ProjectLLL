using System.Collections;
using UnityEngine;

public class CameraChanger : MonoBehaviour, IPausable
{
    [SerializeField] private GameObject _gameCamera;
    [SerializeField] private GameObject _menuCamera;
    [SerializeField] private float _cameraSwitchDelay = 0.5f;

    private void Awake()
    {
        GameStateManager.Instance.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnStateChanged;
        }
    }

    private void OnStateChanged(GameState state)
    {
        // Меняем камеру для паузы (ESC)
        if (state is PausedState)
        {
            SwitchToMenuCameraImmediately();
        }
        // Во время таймера используем игровую камеру
        else if (state is CountdownState || state is GameplayState)
        {
            StartCoroutine(SwitchToGameCamera());
        }
    }

    private IEnumerator SwitchToGameCamera()
    {
        yield return new WaitForSeconds(_cameraSwitchDelay);
        
        if (_gameCamera != null) _gameCamera.SetActive(true);
        if (_menuCamera != null) _menuCamera.SetActive(false);
    }

    private void SwitchToMenuCameraImmediately()
    {
        if (_menuCamera != null) _menuCamera.SetActive(true);
        if (_gameCamera != null) _gameCamera.SetActive(false);
    }

    public void SetPaused(bool paused)
    {
        // Логика паузы уже обрабатывается в OnStateChanged
    }
}