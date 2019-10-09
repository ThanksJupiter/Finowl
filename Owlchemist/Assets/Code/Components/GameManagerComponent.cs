public class GameManagerComponent : BaseComponent
{
    public CartInteraction cartInteraction;
    public bool isInsideCart { get; set; }

public delegate void RestartDelegate();
    public RestartDelegate OnRestart;

    public delegate void VictoryDelegate();
    public VictoryDelegate OnVictory;

    public delegate void DeathDelegate();
    public DeathDelegate OnDeath;


    public delegate void StopGameTickDelegate();
    public StopGameTickDelegate OnStopGameTick;

    public delegate void StartGameTickDelegate();
    public StartGameTickDelegate OnStartGameTick;
}
