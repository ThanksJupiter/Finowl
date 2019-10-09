using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : BaseSystem
{
    public Filter[] filters;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            MovementComponent mc,
            InputComponent ic
            )
        {
            this.id = id;

            gameObject = go;
            movementComponent = mc;
            inputComponent = ic;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public MovementComponent movementComponent;
        public InputComponent inputComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            MovementComponent mc = objects[i].GetComponent<MovementComponent>();
            InputComponent ic = objects[i].GetComponent<InputComponent>();

            if (mc && ic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, mc, ic));
            }
        }

        filters = tmpFilters.ToArray();
        SubscribeToAnimationComponentEvents();
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        //inputComponent.OnAButtonDown += TriggerActionAnimation;
    }

    private void SubscribeToAnimationComponentEvents()
    {
        player.animationComponent.OnPlayGatherAnimation += TriggerGatherAnimation;
        //player.animationComponent.OnPlayAttackAnimation += TriggerAttackAnimation;
        player.animationComponent.OnPlayThrowAnimation += TriggerThrowAnimation;
        player.animationComponent.OnPlayTorchToggleAnimation += TriggerToggleTorchAnimation;
        player.animationComponent.OnPlayDeathAnimation += SetAnimIsDeadTrue;
        player.healthComponent.OnPlayerTakeDamage += TriggerTakeDamageAnimation;
        player.healthComponent.OnPlayerDied += SetAnimIsDeadTrue;

        player.healthComponent.OnPlayerRessurected += SetAnimIsDeadFalse;
    }

    private void TriggerGatherAnimation()
    {
        player.animationComponent.animator.SetTrigger("Gather");
        player.inputComponent.PulseVibrate(.1f);
    }

    private void TriggerAttackAnimation()
    {
        player.animationComponent.animator.SetTrigger("Attack");
    }

    private void TriggerToggleTorchAnimation()
    {
        player.animationComponent.animator.SetTrigger("Toggle Torch");
    }

    private void TriggerRechargeTorchAnimation()
    {
        player.animationComponent.animator.SetTrigger("Recharge Torch");
    }

    private void TriggerTakeDamageAnimation()
    {
        player.inputComponent.PulseVibrate(.5f);
        player.animationComponent.animator.SetTrigger("Take Damage");
        player.movementComponent.SetMovementAllowed(false);
    }

    private void TriggerThrowAnimation()
    {
        player.animationComponent.animator.SetTrigger("Throw");
        player.movementComponent.SetMovementAllowed(false);
    }

    private void SetAnimIsDeadTrue()
    {
        player.inputComponent.PulseVibrate(1);
        player.animationComponent.animator.SetBool("IsDead", true);
        player.movementComponent.alive = false;
    }

    private void SetAnimIsDeadFalse()
    {
        Debug.Log("Setting player dead = false");
        player.animationComponent.animator.SetBool("IsDead", false);
        player.movementComponent.alive = false;
    }

    private void DebugAPressed()
    {
        Debug.Log("A pressed");
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            MovementComponent moveComp = filter.movementComponent;
            InputComponent inputComp = filter.inputComponent;

            // ----- logic -----

            if (moveComp.velocity.magnitude > 0)
            {
                player.animationComponent.animator.SetBool("IsWalking", true);
            }
            else
            {
                player.animationComponent.animator.SetBool("IsWalking", false);
            }
        }
    }
}
