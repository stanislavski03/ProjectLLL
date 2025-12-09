using UnityEngine;
using Cinemachine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _gameVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera _menuVirtualCamera;

    public static CameraChanger Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    


    private void Start()
    {
        GameStateManager.Instance.OnPauseStateChanged += OnPauseChanged;
        
        InitializeCameraPriorities();
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPauseStateChanged -= OnPauseChanged;
        }
    }

    private void InitializeCameraPriorities()
    {
        _gameVirtualCamera.Priority = 10;
        _menuVirtualCamera.Priority = 0;
    }

    private void OnPauseChanged(bool isPaused, GameStateManager.PauseType pauseType)
    {
        if (isPaused && pauseType == GameStateManager.PauseType.EscPause)
        {
            SwitchToMenuCamera();
        }
        else
        {
            SwitchToGameCamera();
        }
    }

    public void SwitchToGameCamera()
    {
        _gameVirtualCamera.Priority = 10;
        _menuVirtualCamera.Priority = 0;
    }

    public void SwitchToMenuCamera()
    {
        _menuVirtualCamera.Priority = 10;
        _gameVirtualCamera.Priority = 0;
    }
}