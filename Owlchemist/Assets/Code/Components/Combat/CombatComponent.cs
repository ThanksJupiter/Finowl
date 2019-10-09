using System.Collections.Generic;
using UnityEngine;

public class CombatComponent : BaseComponent
{
    public int damage = 5;
    public float projectileSpeed = 2f;
    public float hitThreshold = 1f;

    public bool isEngagedInCombat { get; set; }
    public bool isSwiping { get; set; }
    public bool isRunning { get; set; }

    public float minDarknessVibrateTime = 1f;
    public float maxDarknessVibrateTime = 5f;

    public float nextVibrateTime { get; set; }
    public float currentVibrateTime { get; set; }

    public List<Projectile> projectileList { get; set; }
    public Projectile[] projectiles { get; set; }

    private Animator animator;

/*    InputComponent inputComponent = gameObject.GetComponent<InputComponent>();*/

    private void Awake()
    {
        projectileList = new List<Projectile>();
        projectiles = new Projectile[0];

        animator = GetComponent<Animator>();
    }

    public void SetIsEngaged(bool value)
    {
        isEngagedInCombat = value;
    }

//     public void IsSwiping(bool isSwiping)
// {
//     if (GetComponent<InputComponent>().OnXButtonDown)
//     {
// 
//     }
// }
}



public class Projectile
{
    public Projectile(HealthComponent target,Vector3 currentPosition, Vector3 targetPosition, int damage, float speed)
    {
        this.target = target;
        this.currentPosition = currentPosition;
        startPosition = currentPosition;
        this.targetPosition = targetPosition;
        this.damage = damage;
        this.speed = speed;

        lerpAlpha = 0;
    }

    public void IncreaseAlpha(float t)
    {
        lerpAlpha += t;
    }

    public HealthComponent target;
    public Vector3 currentPosition;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float lerpAlpha;
    public int damage;
    public float speed;
}
