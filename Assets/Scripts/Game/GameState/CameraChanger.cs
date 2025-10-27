using UnityEngine;
using System.Collections;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private GameObject _gameCamera;
    [SerializeField] private GameObject _menuCamera;

    private void Start()
    {
        GameStateManager.Instance.OnPauseStateChanged += OnPauseChanged;
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPauseStateChanged -= OnPauseChanged;
        }
    }

    private void OnPauseChanged(bool isPaused, GameStateManager.PauseType pauseType)
    {
        
        if (isPaused && pauseType == GameStateManager.PauseType.EscPause)
        {
            
            SwitchToMenuCameraImmediately();
        }
        else
        {
            SwitchToGameCameraImmediately();
        }
    }

    private void SwitchToGameCameraImmediately()
    {
        _gameCamera.SetActive(true);
        _menuCamera.SetActive(false);
    }

    private void SwitchToMenuCameraImmediately()
    {
        _menuCamera.SetActive(true);
        _gameCamera.SetActive(false);
    }
}