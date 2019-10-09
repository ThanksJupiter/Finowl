using System.Collections.Generic;
using UnityEngine;

public class InteractorComponent : BaseComponent
{
    public InteractMode interactMode { get; set; }

    public bool isInteracting { get; set; }

    public bool isPlacingPlanks { get; set; }
    public bool isAttacking { get; set; }

    public BaseComponent currentInteractable { get; set; }

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask groundMask;

    public List<InteractableComponent> nearbyInteractables = new List<InteractableComponent>();

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void SetInteractMode(InteractMode newMode)
    {
        interactMode = newMode;
        if (newMode == InteractMode.Combat)
        {
            GetComponent<CombatComponent>()?.SetIsEngaged(true);
        }
        else
        {
            GetComponent<CombatComponent>()?.SetIsEngaged(false);
        }
    }
}

public enum InteractMode
{
    None,
    PlacingPlanks,
    Combat,
    Object
}