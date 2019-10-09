using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

/// <summary>
/// Stores all systems and tries to initialize them with every child of this transform
/// </summary>
public class World : MonoBehaviour
{
    public bool shouldTick = true;

    public GameObject playerObject;

    private BaseSystem[] systems;
    private InputSystem inputSystem;

    bool initialized = false;
    StoryBeatSystem sbs;

    public Scene[] scenes;

    private void Awake()
    {
        Scene main = SceneManager.GetSceneAt(0);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            /*Debug.Log(i);
            Scene sceneToLoad = SceneManager.GetSceneAt(i);
            if (!sceneToLoad.isLoaded)
            {
                SceneManager.LoadScene(sceneToLoad.name, LoadSceneMode.Additive);
            }*/
        }

        //SceneManager.SetActiveScene(main);


        Assert.IsNotNull(playerObject, "Player gameobject not assigned in [World], please put the player there :: )))");

        // nothing will happen without this
        initialized = playerObject != null;

        systems = GetComponentsInChildren<BaseSystem>();
    }

    private IEnumerator LoadLevel(Scene scene)
    {
        if (!scene.isLoaded) 
        { 
            
        }
        yield return null;
    }

    private void Start()
    {
        inputSystem = new InputSystem();

        Initialize();
    }

    public void Initialize()
    {
        if (!initialized) { return; }

        Transform[] worldObjects = GetComponentsInChildren<Transform>();

        inputSystem.Initialize(worldObjects);

        InputComponent inpC = playerObject.GetComponent<InputComponent>();
        HealthComponent healthC = playerObject.GetComponent<HealthComponent>();
        InteractorComponent intC = playerObject.GetComponent<InteractorComponent>();
        MovementComponent moveC = playerObject.GetComponent<MovementComponent>();
        InventoryComponent invC = playerObject.GetComponent<InventoryComponent>();
        GameManagerComponent gmC = playerObject.GetComponent<GameManagerComponent>();
        AnimationComponent animC = playerObject.GetComponent<AnimationComponent>();
        PlayerStoryComponent storyBeatC = playerObject.GetComponent<PlayerStoryComponent>();
        PlayerEventComponent eventC = playerObject.GetComponent<PlayerEventComponent>();
        GatheringComponent gatC = playerObject.GetComponent<GatheringComponent>();
        UIComponent uiC = playerObject.GetComponent<UIComponent>();
        NavMeshComponent nmC = playerObject.GetComponent<NavMeshComponent>();
        LightComponent lC = playerObject.GetComponent<LightComponent>();
        VFXComponent vfxC = playerObject.GetComponent<VFXComponent>();
        CombatComponent cC = playerObject.GetComponent<CombatComponent>();
        AudioComponent aC = playerObject.GetComponent<AudioComponent>();
        ConditionsComponent conC = playerObject.GetComponent<ConditionsComponent>();

        PlayerFilter pf = new PlayerFilter(
            playerObject, inpC, healthC, intC,
            moveC, invC, gmC, animC, storyBeatC,
            eventC, gatC, uiC, nmC, lC, vfxC,
            cC, aC, conC);

        sbs = GetComponent<StoryBeatSystem>();

        for (int i = 0; i < systems.Length; i++)
        {
            systems[i].player = pf;
            systems[i].Initialize(worldObjects);
            systems[i].SetupInputComponent(inputSystem.inputComponent);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        inputSystem.Tick(deltaTime);

        if (shouldTick && initialized)
        {
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].Tick(deltaTime);
            }
        }
        else
        {
            sbs.Tick(deltaTime);
        }
    }

    // for controller vibration
    private void FixedUpdate()
    {
        inputSystem.FixedTick();
    }
}
