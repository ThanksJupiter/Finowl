using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISystem : BaseSystem
{
    public Canvas playerCanvas;
    public Canvas pauseCanvas;

    private Filter[] filters;
    private GameObject currentElement;

    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            InputComponent ic
            )
        {
            this.id = id;

            gameObject = go;
            inputComponent = ic;
        }

        public int id;

        public GameObject gameObject;
        public InputComponent inputComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            InputComponent ic = objects[i].GetComponent<InputComponent>();

            if (ic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ic));
            }
        }

        filters = tmpFilters.ToArray();
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        inputComponent.OnPauseButtonDown += TogglePauseMenu;
        player.gameManagerComponent.OnDeath += EnableDeathScreen;
        player.eventComponent.OnDisplayUIElement += DisplayUIElement;
        player.eventComponent.OnHideUIElement += HideUIElement;
        player.eventComponent.OnDisplayGatherPrompt += DisplayGatherPrompt;
        player.eventComponent.OnHidePreviousGatherPrompt += HidePreviousGatherPrompt;

        player.healthComponent.OnPlayerTakeDamage += RemoveHeart;
        player.healthComponent.OnPlayerRestoreDamage += RestoreHeart;
        player.healthComponent.OnPlayerTakeDamage += IncreaseHealth;
        player.healthComponent.OnPlayerRestoreDamage += DecreaseHealth;
    }

    private void DisplayGatherPrompt(CollectibleComponent collectible)
    {
        string interactString = "hold [A] " + collectible.interactionString;
        player.uiComponent.gatheringPrompt.gatherTXT.text = interactString;
        player.uiComponent.gatheringPrompt.gameObject.SetActive(true);
        player.uiComponent.currentlyDisplayedGatherPrompt = player.uiComponent.gatheringPrompt;
    }

    private void HidePreviousGatherPrompt()
    {
        if (player.uiComponent.currentlyDisplayedGatherPrompt != null)
        {
            player.uiComponent.currentlyDisplayedGatherPrompt.gameObject?.SetActive(false);
            player.uiComponent.currentlyDisplayedGatherPrompt = null;
        }
    }

    private void DisplayUIElement(GameObject newElement, bool hidePrevious)
    {
        if (currentElement && hidePrevious)
        {
            currentElement.SetActive(false);
        }

        currentElement = newElement;
        currentElement.SetActive(true);
    }

    private void DecreaseHealth()
    {
        player.uiComponent.healthIndicator.TriggerHeartPulse(player.healthComponent.currentHealth);
    }

    private void IncreaseHealth()
    {
        player.uiComponent.healthIndicator.TriggerHeartDecay(player.healthComponent.currentHealth);
    }

    private void RemoveHeart()
    {
        player.uiComponent.healthIndicator.RemoveHeart();
    }

    private void RestoreHeart()
    {
        player.uiComponent.healthIndicator.RestoreHeart();
    }

    private void DisplayInteractableUIElement(InteractableComponent interactable)
    {
        
    }

    private void HideUIElement(GameObject element)
    {
        element.SetActive(false);
    }

    private void TogglePauseMenu()
    {
        player.uiComponent.pauseMenu.gameObject.SetActive(!player.uiComponent.pauseMenu.gameObject.activeSelf);
        player.uiComponent.pauseMenu.SubscribeToPause(player);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(player.uiComponent.pauseMenu.defaultButton.gameObject);

        if (player.uiComponent.pauseMenu.gameObject.activeSelf)
        {
            player.animationComponent.animator.speed = 0f;
            player.gameManagerComponent.OnStopGameTick.Invoke();
        }
        else
        {
            player.animationComponent.animator.speed = 1f;
            player.gameManagerComponent.OnStartGameTick.Invoke();
        }
    }

    private void EnableDeathScreen()
    {
        player.uiComponent.deathScreen.gameObject.SetActive(true);
        player.uiComponent.deathScreen.SubscribeToButtonEvents(player);
        //player.animationComponent.animator.speed = 0f;
        player.gameManagerComponent.OnStopGameTick.Invoke();
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            InputComponent inputComp = filter.inputComponent;

            // ----- logic -----

            
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            player.go.GetComponent<MovementComponent>().agent.Warp(player.uiComponent.deathScreen.cartInteraction.cartExitPoint.transform.position);
            player.go.transform.position = player.uiComponent.deathScreen.cartInteraction.cartExitPoint.transform.position;
        }
    }
}
