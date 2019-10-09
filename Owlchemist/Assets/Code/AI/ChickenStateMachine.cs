using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStateMachine : MonoBehaviour
{
    public bool patrolling = false;
    public bool stalking = false;
    public bool retreating = false;
    public bool idle = false;
    public bool attacking = false;
    public bool charging = false;
    public bool seeingPlayer = false;
    public bool stunned = false;

    public void SetPreviousStateFalse()
    {
        patrolling = false;
        stalking = false;
        retreating = false;
        idle = false;
        attacking = false;
        charging = false;
    }
}
