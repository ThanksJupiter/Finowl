using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputComponent : BaseComponent
{
    [Header("Interaction masks")]
    public LayerMask clickableMask;
    public LayerMask objectPlaceMask;
    public LayerMask worldInteractMask;

    [Header("Controller settings")]
    public bool enableVibration = false;

    private Vector2 previousMousePosition;
    private Vector2 mouseDelta;

    public bool avoidPlayer { get; set; }

    //bool playerIndexSet = false;

    public PlayerIndex playerIndex;
    public GamePadState state;
    public GamePadState prevState;

    public Vector2 leftStickInput;
    public Vector2 rightStickInput;

    public delegate void AButtonDelegate();
    public AButtonDelegate OnAButton;

    public delegate void AButtonDownDelegate();
    public AButtonDownDelegate OnAButtonDown;

    public delegate void BButtonDownDelegate();
    public BButtonDownDelegate OnBButtonDown;

    public delegate void XButtonDownDelegate();
    public XButtonDownDelegate OnXButtonDown;

    public delegate void YButtonDownDelegate();
    public YButtonDownDelegate OnYButtonDown;
    public delegate void YButtonUpDelegate();
    public YButtonUpDelegate OnYButtonUp;

    public delegate void PauseButtonDownDelegate();
    public PauseButtonDownDelegate OnPauseButtonDown;

    public delegate void TriggerVibrateDelegate(float amount);
    public TriggerVibrateDelegate OnTriggerVibrate;

    public float rightVibrationAmount { get; set; }
    public float leftVibrationAmount { get; set; }

    public float leftShoulderAxis { get; set; }

    public bool aButtonDown { get; set; }
    public bool bButtonDown { get; set; }
    public bool xButtonDown { get; set; }
    public bool yButtonDown { get; set; }

    public bool leftShoulder { get; set; }
    public bool rightShoulder { get; set; }

    public bool leftShoulderDown { get; set; }
    public bool rightShoulderDown { get; set; }

    public bool leftBumberDown { get; set; }
    public bool rightBumberDown { get; set; }

    public bool leftDPad { get; set; }
    public bool rightDPad { get; set; }
    public bool upDPad { get; set; }
    public bool downDPad { get; set; }
    private void Update()
    {
        CalculateMouseDelta();
    }

    public void PulseVibrate(float amount)
    {
        rightVibrationAmount = amount;
        leftVibrationAmount = amount;
    }

    public void SetRightRumble(float amount)
    {
        rightVibrationAmount = amount;
    }

    public void SetLeftRumble(float amount)
    {
        leftVibrationAmount = amount;
    }

    public Vector3 GetMouseWorldLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, clickableMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public Vector3 GetMouseWorldLocation(out RaycastHit raycastHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, clickableMask))
        {
            raycastHit = hit;
            return hit.point;
        }

        raycastHit = hit;
        return Vector3.zero;
    }

    public Vector3 GetMouseHoverTransformPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, clickableMask))
        {
            return hit.transform.position;
        }

        return Vector3.zero;
    }

    public Vector3 GetMouseHoverTransformPosition(LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            return hit.transform.position;
        }

        return Vector3.zero;
    }

    public bool GetMouseButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }

    public bool GetMouseButton(int button)
    {
        return Input.GetMouseButton(button);
    }

    public Vector3 GetInputDirection()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
    }

    public Vector3 GetLeftStickInput()
    {
        return new Vector3(leftStickInput.x, 0f, leftStickInput.y);
    }

    public Vector3 GetRightStickInput()
    {
        return new Vector3(rightStickInput.x, 0f, rightStickInput.y);
    }

    public Vector3 GetRelativeInputDirection()
    {
        Vector3 worldInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        return transform.TransformDirection(worldInput);
    }

    public float GetTurnDirection()
    {
        return Input.GetAxisRaw("LookHorizontal");
    }

    public Vector2 GetMouseDelta()
    {
        return mouseDelta;
    }


    private void CalculateMouseDelta()
    {
        Vector2 currentMousePosition = Input.mousePosition;

        float xMovement = previousMousePosition.x - currentMousePosition.x;
        float yMovement = previousMousePosition.y - currentMousePosition.y;

        previousMousePosition = Input.mousePosition;

        if (Mathf.Abs(xMovement) != 0.0f || Mathf.Abs(yMovement) != 0.0f)
        {
            mouseDelta = new Vector2(xMovement, yMovement);
        }
        else
        {
            mouseDelta = Vector2.zero;
        }
    }

    public bool GetKey(KeyCode keyCode)
    {
        return Input.GetKey(keyCode);
    }
    public bool GetKeyDown(KeyCode keyCode)
    {
        return Input.GetKeyDown(keyCode);
    }
    public bool GetKeyUp(KeyCode keyCode)
    {
        return Input.GetKeyUp(keyCode);
    }

    public bool GetButtonDown(string buttonName)
    {
        return Input.GetButtonDown(buttonName);
    }
    public bool GetButtonUp(string buttonName)
    {
        return Input.GetButtonUp(buttonName);
    }
    public bool GetButton(string buttonName)
    {
        return Input.GetButton(buttonName);
    }

}

public enum OwlInput
{
    AButton,
    BButton,
    XButton,
    YButton
}
