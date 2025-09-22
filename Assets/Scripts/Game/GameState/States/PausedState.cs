using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PausedState : GameState
{
    public override string StateName => "Paused";
    public override bool ShowCountdown => false;
    public override bool ChangeCamera => true;
    public override bool IsPaused => true;
    public override bool CanPauseWithESC => true;
    
    public bool ShouldShowCountdownOnExit { get; set; } = true;

    public override void EnterState()
    {
        stateManager.SetPauseForAllSystems(true);
        ShouldShowCountdownOnExit = true;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RequestResume();
        }
    }

    public override void RequestResume()
    {
        stateManager.SwitchState(new CountdownState(this));
    }
}