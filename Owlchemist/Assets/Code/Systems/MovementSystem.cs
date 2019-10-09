using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : BaseSystem
{
    public Filter[] filters;
    public GridComponent gridComp;
    public Cinemachine.CinemachineVirtualCamera playerCamera;

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
        // list because I don't know size here////
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

        gridComp = GetComponentInChildren<GridComponent>();
    }
    
    public override void SetupInputComponent(InputComponent inputComponent)
    {
        //inputComponent.OnAButtonDown += DebugAPressed;
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

            if (moveComp.alive && moveComp.movementAllowed)
            {
                Move(filter, deltaTime);
                Rotate(inputComp, moveComp);
            }
        }
    }

    private void Move(Filter filter, float deltaTime)
    {
        Vector3 inputDirection = playerCamera.transform.TransformDirection(filter.inputComponent.GetInputDirection()).normalized;
        filter.movementComponent.velocity = inputDirection;

        if ((filter.inputComponent.leftShoulderAxis - filter.movementComponent.blendAlpha) > filter.movementComponent.lerpThreshold)
        {
            filter.movementComponent.blendAlpha += filter.movementComponent.accelerationSpeed * deltaTime;
        }
        else if ((filter.movementComponent.blendAlpha - filter.inputComponent.leftShoulderAxis) > filter.movementComponent.lerpThreshold)
        {
            filter.movementComponent.blendAlpha -= filter.movementComponent.accelerationSpeed * deltaTime;
        }

        player.combatComponent.isRunning = player.inputComponent.leftShoulder && inputDirection.magnitude > float.Epsilon;
        filter.movementComponent.blendAlpha = Mathf.Clamp(filter.movementComponent.blendAlpha, 0, 1);

        float speed = Mathf.Lerp(
            filter.movementComponent.walkSpeed,
            filter.movementComponent.runSpeed,
            filter.movementComponent.useAcceleration ? filter.movementComponent.blendAlpha : filter.inputComponent.leftStickInput.magnitude);

        Vector3 newPos = inputDirection * speed * deltaTime;
        float animSpeed = Mathf.InverseLerp(0, filter.movementComponent.runSpeed, (inputDirection * speed).magnitude);
        player.animationComponent.animator.SetFloat("MoveBlend", animSpeed);
        filter.movementComponent.agent.Move(newPos);
    }

    private void Rotate(InputComponent ic, MovementComponent mc)
    {
        if (ic.GetInputDirection().magnitude > 0)
        {
            Vector3 flatRot = new Vector3(mc.velocity.x, 0f, mc.velocity.z);
            float dot = Vector3.Dot(flatRot, mc.gameObject.transform.right);
            mc.GetOwnerGO().transform.Rotate(Vector3.up, dot * mc.turnSpeed);
        }
    }
}
