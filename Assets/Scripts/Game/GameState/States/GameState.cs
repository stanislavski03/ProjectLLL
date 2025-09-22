using UnityEngine;

public abstract class GameState
{
    public abstract string StateName { get; }
    public virtual bool CanPauseWithESC => false;
    public virtual bool ShowCountdown => false;
    public virtual bool ChangeCamera => false;
    public virtual bool IsPaused => false;
    
    protected GameStateManager stateManager;
    
    public virtual void Initialize(GameStateManager manager)
    {
        stateManager = manager;
    }
    
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    
    public virtual void RequestPause() { }
    public virtual void RequestResume() { }
    public virtual void RequestLevelUp() { }
}