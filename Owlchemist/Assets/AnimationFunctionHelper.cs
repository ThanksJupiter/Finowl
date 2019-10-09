using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctionHelper : MonoBehaviour
{
    public LightComponent plc;
    public MovementComponent mc;
    public CombatComponent cc;
    public InputComponent ip;
    public AudioComponent ac;

    public void ToggleLight()
    {
        if (plc.currentFuel > 0)
        {
            plc.SetLightEnabled(!plc.IsLightEnabled());
            ip.PulseVibrate(.2f);
            plc.PlayFizzle();
            ac.OnPlayTorchToggleSound?.Invoke();
        }
        else
        {
            plc.PlayFizzle();
            ip.PulseVibrate(.1f);
            ac.OnPlayTorchToggleSound?.Invoke();
        }

        mc.SetMovementAllowed(true);
    }

    public void RestoreMovement()
    {
        Debug.Log("Restoring movement");
        mc.SetMovementAllowed(true);
    }

    public void SetAttackingTrue()
    {
        cc.isSwiping = true;
    }

    public void SetAttackingFalse()
    {
        cc.isSwiping = false;
        mc.SetMovementAllowed(true);
    }
}
