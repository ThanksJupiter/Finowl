using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSwipe : MonoBehaviour
{
    private Animator animator;
    public Collider torchCollider;
   //public InputComponent inputComponent;

    [SerializeField]
    public bool attacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        torchCollider = GetComponent<Collider>();
        //InputComponent inputComponent = gameObject.GetComponentInParent<InputComponent>();

        attacking = false;

//         if (inputComponent == null)
//             return;

        /*SetupInputComponent();*/
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PerformTorchSwipe();
            if (torchCollider.enabled)
                attacking = true;
        }
        else
            attacking = false;

    }

//     public override void SetupInputComponent(InputComponent inputComponent)
//     {
//         inputComponent.OnXButtonDown += PerformTorchSwipe();
//     }

    public void PerformTorchSwipe()
    {
        Debug.Log(this.name + " attack!");
        animator.SetTrigger("SwipeAttack");
    }
}