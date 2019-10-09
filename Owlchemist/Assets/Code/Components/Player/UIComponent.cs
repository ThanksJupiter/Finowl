using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponent : BaseComponent
{
    public GatherPrompt gatheringPrompt;
    public InteractPrompt interactPrompt;
    public GameObject endScreenObj;
    public GroceryList groceryList;
    public TorchIndicator torchIndicator;
    public SisterLetter sisterLetter;
    public HealthIndicator healthIndicator;
    public GameObject restartButton;
    public PauseMenu pauseMenu;
    public DeathScreen deathScreen;

    public GatherPrompt currentlyDisplayedGatherPrompt { get; set; }
}
