public class LevelUpState : GameState
{
    public override string StateName => "LevelUpPaused";
    public override bool IsPaused => true;
    
    public override void EnterState()
    {
        stateManager.SetPauseForGameplaySystems(true);
        
        LevelUpController levelUpController = stateManager.GetLevelUpController();
        // if (levelUpController != null)
        // {
        //     levelUpController.OnLevelUp();
        // }
    }
    
    public override void Update()
    {
    }
    
    public override void RequestResume()
    {
        stateManager.SwitchState(new GameplayState());
    }
    
    public override void ExitState()
    {
        LevelUpController levelUpController = stateManager.GetLevelUpController();
        if (levelUpController != null)
        {
            levelUpController.HideLevelUpOptions();
        }
    }
}