using UnityEngine;

[System.Serializable]
public struct PlayerFilter
{
    public PlayerFilter(
            GameObject go,
            InputComponent inputC,
            HealthComponent healthC,
            InteractorComponent interactC,
            MovementComponent moveC,
            InventoryComponent inventoryC,
            GameManagerComponent gmc,
            AnimationComponent animC,
            PlayerStoryComponent psC,
            PlayerEventComponent peC,
            GatheringComponent gC,
            UIComponent uiC,
            NavMeshComponent nmC,
            LightComponent lC,
            VFXComponent vfxC,
            CombatComponent cC,
            AudioComponent aC,
            ConditionsComponent conC)
    {
        this.go = go;
        inputComponent = inputC;
        healthComponent = healthC;
        interactorComponent = interactC;
        movementComponent = moveC;
        inventoryComponent = inventoryC;
        gameManagerComponent = gmc;
        animationComponent = animC;
        playerStoryComponent = psC;
        eventComponent = peC;
        gatheringComponent = gC;
        uiComponent = uiC;
        navMeshComponent = nmC;
        lightComponent = lC;
        vfxComponent = vfxC;
        combatComponent = cC;
        audioComponent = aC;
        conditionsComponent = conC;
    }

    public GameObject go;
    public InputComponent inputComponent;
    public HealthComponent healthComponent;
    public InteractorComponent interactorComponent;
    public MovementComponent movementComponent;
    public InventoryComponent inventoryComponent;
    public GameManagerComponent gameManagerComponent;
    public AnimationComponent animationComponent;
    public PlayerStoryComponent playerStoryComponent;
    public PlayerEventComponent eventComponent;
    public GatheringComponent gatheringComponent;
    public UIComponent uiComponent;
    public NavMeshComponent navMeshComponent;
    public LightComponent lightComponent;
    public VFXComponent vfxComponent;
    public CombatComponent combatComponent;
    public AudioComponent audioComponent;
    public ConditionsComponent conditionsComponent;
}
