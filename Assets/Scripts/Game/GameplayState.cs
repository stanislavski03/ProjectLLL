public class GameplayState : GameState
{
    public override string StateName => "Gameplay";
    public override bool CanPauseWithESC => true;
    public override bool IsPaused => false;
    public override bool ShowCountdown => false; // ← ДОБАВИТЬ ЭТУ СТРОЧКУ
    
    public override void EnterState()
    {
        stateManager.SetPauseForAllSystems(false);
    }
    
    public override void RequestPause()
    {
        stateManager.SwitchState(new PausedState());
    }
    
    public override void RequestLevelUp()
    {
        stateManager.SwitchState(new LevelUpState());
    }
}