using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : BaseComponent
{
    public Animator animator;

    public delegate void GatherAnimationDelegate();
    public GatherAnimationDelegate OnPlayGatherAnimation;

    public delegate void AttackAnimationDelegate();
    public AttackAnimationDelegate OnPlayAttackAnimation;

    public delegate void ThrowAnimationDelegate();
    public ThrowAnimationDelegate OnPlayThrowAnimation;

    public delegate void DeathAnimationDelegate();
    public DeathAnimationDelegate OnPlayDeathAnimation;

    public delegate void ToggleTorchAnimationDelegate();
    public ToggleTorchAnimationDelegate OnPlayTorchToggleAnimation;

    public delegate void RechargeTorchAnimationDelegate();
    public RechargeTorchAnimationDelegate OnRechargeTorchAnimation;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }
}
