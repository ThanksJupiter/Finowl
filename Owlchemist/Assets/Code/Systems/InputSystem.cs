using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputSystem
{
    public Filter[] filters;
    public InputComponent inputComponent;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
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

        // why not
        public int id;

        public GameObject gameObject;
        public InputComponent inputComponent;
    }

    public void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            InputComponent ic = objects[i].GetComponent<InputComponent>();

            if (ic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ic));
                ic.playerIndex = 0;
                inputComponent = ic;
            }
        }

        filters = tmpFilters.ToArray();
    }

    public void FixedTick()
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            InputComponent inputComp = filter.inputComponent;

            // ----- logic -----

            /*GamePad.SetVibration(
                inputComp.playerIndex,
                inputComp.state.Triggers.Left,
                inputComp.state.Triggers.Right
                );*/

            GamePad.SetVibration(
                inputComp.playerIndex,
                inputComp.leftVibrationAmount,
                inputComp.rightVibrationAmount
            );
        }
    }

    public void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            InputComponent inputComp = filter.inputComponent;

            // ----- logic -----

            inputComp.prevState = inputComp.state;
            inputComp.state = GamePad.GetState(inputComp.playerIndex);

            if (inputComp.state.Buttons.A == ButtonState.Pressed)
            {
                inputComp.OnAButton?.Invoke();
            }

            inputComp.leftDPad = inputComp.prevState.DPad.Left == ButtonState.Released && inputComp.state.DPad.Left == ButtonState.Pressed;
            inputComp.rightDPad = inputComp.prevState.DPad.Right == ButtonState.Released && inputComp.state.DPad.Right == ButtonState.Pressed;
            inputComp.upDPad = inputComp.prevState.DPad.Up == ButtonState.Released && inputComp.state.DPad.Up == ButtonState.Pressed;
            inputComp.downDPad = inputComp.prevState.DPad.Down == ButtonState.Released && inputComp.state.DPad.Down == ButtonState.Pressed;
            
            inputComp.aButtonDown = inputComp.prevState.Buttons.A == ButtonState.Released && inputComp.state.Buttons.A == ButtonState.Pressed;
            inputComp.bButtonDown = inputComp.prevState.Buttons.B == ButtonState.Released && inputComp.state.Buttons.B == ButtonState.Pressed;
            inputComp.xButtonDown = inputComp.prevState.Buttons.X == ButtonState.Released && inputComp.state.Buttons.X == ButtonState.Pressed;
            inputComp.yButtonDown = inputComp.prevState.Buttons.Y == ButtonState.Released && inputComp.state.Buttons.Y == ButtonState.Pressed;

            
            if (inputComp.aButtonDown || Input.GetKeyDown(KeyCode.A))
            {
                inputComp.OnAButtonDown?.Invoke();
            }

            if (inputComp.bButtonDown || Input.GetKeyDown(KeyCode.B))
            {
                inputComp.OnBButtonDown?.Invoke();
            }

            if (inputComp.prevState.Buttons.X == ButtonState.Released && inputComp.state.Buttons.X == ButtonState.Pressed)
            {
                inputComp.OnXButtonDown?.Invoke();
            }
            if (inputComp.prevState.Buttons.Y == ButtonState.Released && inputComp.state.Buttons.Y == ButtonState.Pressed)
            {
                inputComp.OnYButtonDown?.Invoke();
            }
            if (inputComp.prevState.Buttons.Y == ButtonState.Released)
            {
                inputComp.OnYButtonUp?.Invoke();
            }
            if (inputComp.prevState.Buttons.Start == ButtonState.Released && inputComp.state.Buttons.Start == ButtonState.Pressed)
            {
                inputComp.OnPauseButtonDown?.Invoke();
            }

            inputComp.leftShoulder = inputComp.state.Triggers.Left > 0f;
            inputComp.rightShoulder = inputComp.state.Triggers.Right > 0f;

            inputComp.leftShoulderAxis = inputComp.state.Triggers.Left;

            inputComp.leftShoulderDown = inputComp.prevState.Triggers.Left == 0f && inputComp.state.Triggers.Left > 0f;
            inputComp.rightShoulderDown = inputComp.prevState.Triggers.Right == 0f && inputComp.state.Triggers.Right > 0f;

            inputComp.leftBumberDown = inputComp.prevState.Buttons.LeftShoulder == ButtonState.Released && inputComp.state.Buttons.LeftShoulder == ButtonState.Pressed;

            inputComp.leftStickInput = new Vector2(inputComp.state.ThumbSticks.Left.X, inputComp.state.ThumbSticks.Left.Y);
            inputComp.rightStickInput = new Vector2(inputComp.state.ThumbSticks.Right.X, inputComp.state.ThumbSticks.Right.Y);

            inputComp.rightVibrationAmount -= Time.deltaTime;
            inputComp.leftVibrationAmount -= Time.deltaTime;
        }
    }
}
