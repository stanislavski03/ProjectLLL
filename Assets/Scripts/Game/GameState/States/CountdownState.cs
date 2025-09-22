using System.Collections;
using UnityEngine;

public class CountdownState : GameState
{
    public override string StateName => "Countdown";
    public override bool ShowCountdown => true;
    public override bool IsPaused => true;
    public override bool ChangeCamera => true;
    
    private PausedState previousPausedState;
    
    public CountdownState(PausedState pausedState)
    {
        previousPausedState = pausedState;
    }
    
    public override void EnterState()
    {
        stateManager.SetPauseForAllSystems(true);
        
        CountdownController countdownController = Object.FindObjectOfType<CountdownController>();
        if (countdownController != null)
        {
            countdownController.StartCountdown(() => 
            {
                stateManager.SwitchState(new GameplayState());
            });
        }
        else
        {
            stateManager.SwitchState(new GameplayState());
        }
    }
}