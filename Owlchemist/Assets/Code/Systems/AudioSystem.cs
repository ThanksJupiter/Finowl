using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : BaseSystem
{
    public Filter[] filters;

    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            MovementComponent mc,
            InputComponent ic,
            LightComponent lc
            )
        {
            this.id = id;

            gameObject = go;
            movementComponent = mc;
            inputComponent = ic;
            lightComponent = lc;
        }

        public int id;

        public GameObject gameObject;
        public MovementComponent movementComponent;
        public InputComponent inputComponent;
        public LightComponent lightComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            MovementComponent mc = objects[i].GetComponent<MovementComponent>();
            InputComponent ic = objects[i].GetComponent<InputComponent>();
            LightComponent lc = objects[i].GetComponent<LightComponent>();
            if (mc && ic && lc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, mc, ic, lc));
            }
        }

        filters = tmpFilters.ToArray();
    }

    public override void Tick(float deltaTime)
    {
        /*for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            MovementComponent moveComp = filter.movementComponent;
            InputComponent inputComp = filter.inputComponent;

            // ----- logic -----
            
        }*/


    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        player.audioComponent.OnPlayTorchToggleSound += PlayTorchToggle;
        //player.animationComponent.OnPlayTorchToggleAnimation += PlayTorchToggle;
        player.eventComponent.OnGatherResource += PlayHarvest;
        //player.lightComponent.
        //player.gameManagerComponent.On..

        player.animationComponent.OnPlayAttackAnimation += PlayTorchSwipe;
    }
    private void PlayTorchSwipe()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.torchSwipe);
    }
    private void PlayTorchIlde()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.torchIdle);
    }
    private void PlayHarvest()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.harvesSchroom);
    }
    private void PlayTorchToggle()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.torchOn, 1.1f);
    }
    private void PlayStepSound()
    {
        Debug.Log("FootSteep");
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.footsteps[Random.Range(0, player.audioComponent.footsteps.Length - 1)]);
    }
    private void PlayCraft()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.cartCraft);
    }
    private void PlayPage()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.pageTurns[Random.Range(0, player.audioComponent.pageTurns.Length - 1)]);
    }
    private void PlayCartExit()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.door);
    }
    private void PlayCompleteQuest()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.completQuest);
    }
    private void PlayNewQuest()
    {
        player.audioComponent.oneShotSource.PlayOneShot(player.audioComponent.newQuest);
    }
}
