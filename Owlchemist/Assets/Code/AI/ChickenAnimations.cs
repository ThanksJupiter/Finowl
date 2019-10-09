using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenAnimations : MonoBehaviour
{
    private Animator anim;
    private ChickenAIComponent chickAI;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        chickAI = GetComponent<ChickenAIComponent>();

        chickAI.OnTriggerLook += TriggerLookAroundAnim;
        chickAI.OnTriggerStagger += TriggerStaggerAnim;
        chickAI.OnTriggerStomp += TriggerStompAnim;

        chickAI.OnSetRunning += SetIsRunning;
        chickAI.OnSetWalking += SetIsWalking;
        chickAI.OnSetStalking += SetIsStalking;

        Assert.IsNotNull(anim, "Animator is missing on Enemy");
    }

    private void TriggerLookAroundAnim()
    {
        anim.SetTrigger("LookAround");
    }

    private void TriggerStaggerAnim()
    {
        anim.SetTrigger("Stagger");
    }

    private void TriggerStompAnim()
    {
        anim.SetTrigger("Stomp");
    }

    private void SetIsRunning()
    {
        anim.SetBool("IsRunning", true);
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsStalking", false);
    }

    private void SetIsWalking()
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsWalking", true);
        anim.SetBool("IsStalking", false);
    }

    private void SetIsStalking()
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsStalking", true);
    }
}
