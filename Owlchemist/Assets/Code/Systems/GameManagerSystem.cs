using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManagerSystem : BaseSystem
{
    public Filter[] filters;
    private GameManagerComponent gameManagerComponent;
    private GOAPSystem goapSystem;
    public GameObject RespawnButton;
    TravelComponent travelComponent;
    CartComponent transportableComponent;

    private World world;

    private void Awake()
    {
        world = GetComponent<World>();
        Assert.IsNotNull(world, "[GameManagerSystem] is without [World], this is not even good");
    }

    private void OnDisable()
    {
        for (int i = 0; i < filters.Length; i++)
        {
            filters[i].gameManagerComponent.OnRestart -= Restart;
            filters[i].gameManagerComponent.OnVictory -= VictoryScreen;
            filters[i].gameManagerComponent.OnDeath -= RestartFromDeath;

            filters[i].gameManagerComponent.OnStartGameTick -= StartGameTick;
            filters[i].gameManagerComponent.OnStopGameTick -= StopGameTick;
        }
    }

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            GameManagerComponent gmc
            )
        {
            this.id = id;

            gameObject = go;
            gameManagerComponent = gmc;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public GameManagerComponent gameManagerComponent;
    }

    // HACK
    MovementComponent movementComp;
    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            GameManagerComponent gmc = objects[i].GetComponent<GameManagerComponent>();

            if (gmc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, gmc));
                gmc.OnRestart += Restart;
                gmc.OnVictory += VictoryScreen;
                //gmc.OnDeath += RestartFromDeath;

                gmc.OnStartGameTick += StartGameTick;
                gmc.OnStopGameTick += StopGameTick;
            }
        }

        filters = tmpFilters.ToArray();
        travelComponent = GetComponentInChildren<TravelComponent>();
        transportableComponent = GetComponentInChildren<CartComponent>();
        goapSystem = GetComponent<GOAPSystem>();
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            GameManagerComponent gameComp = filter.gameManagerComponent;
            
            // ----- logic -----
            if(travelComponent.gameObject.GetComponent<HealthComponent>())
            {
                if(travelComponent.gameObject.GetComponent<HealthComponent>().currentHealth <= 0)
                {
                    player.uiComponent.restartButton.SetActive(true);
                    //travelComponent.gameObject.GetComponent<MovementComponent>().alive = false;
                    //travelComponent.gameObject.GetComponent<LightComponent>().SetLightEnabled(true);
                }
            }

            /*if (!player.movementComponent.alive)
            {
                if (Input.GetButtonDown("Start"))
                {
                    RestartFromDeath();
                }
            }*/
        }
    }

    public void StartGameTick()
    {
        world.shouldTick = true;
    }

    public void StopGameTick()
    {
        world.shouldTick = false;
    }

    private void OnPlayerDeath()
    {
        player.gameManagerComponent.OnDeath?.Invoke();
    }

    public void RestartFromDeath()
    {
        //player.uiComponent.restartButton.SetActive(false);
        player.inventoryComponent.EmptyInventory();
        player.uiComponent.healthIndicator.Reset();
        player.animationComponent.animator.SetBool("IsDead", false);
        player.healthComponent.currentHealth = player.healthComponent.maxHealth;
        travelComponent.gameObject.GetComponent<MovementComponent>().alive = true;

        travelComponent.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(transportableComponent.transform.position + Vector3.right * 5f);
        travelComponent.gameObject.GetComponent<HealthComponent>().currentHealth = travelComponent.gameObject.GetComponent<HealthComponent>().maxHealth;
        travelComponent.gameObject.GetComponent<LightComponent>().Reset();
        //Clear inventory
        //blackoutscreen?
        //
    }
    private void VictoryScreen()
    {
        //Make victory screen logic 
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //lose - reset stats and go back to cart

    }
    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //lose - reset stats and go back to cart
        //Make restart logic 
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        player.healthComponent.OnPlayerDied += OnPlayerDeath;
    }
}
