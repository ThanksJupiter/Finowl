using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventComponent : BaseComponent
{
    public bool hasTakenFogDamage { get; set; }

    public delegate void DisplayUIElementDelegate(GameObject element, bool hidePrevious);
    public DisplayUIElementDelegate OnDisplayUIElement;

    public delegate void HideUIElementDelegate(GameObject element);
    public HideUIElementDelegate OnHideUIElement;

    public delegate void DisplayGatherPromptDelegate(CollectibleComponent collectible);
    public DisplayGatherPromptDelegate OnDisplayGatherPrompt;

    public delegate void HidePreviousGatherPromptDelegate();
    public HidePreviousGatherPromptDelegate OnHidePreviousGatherPrompt;

    public delegate void GatherResourceDelegate();
    public GatherResourceDelegate OnGatherResource;

    public delegate void RefillOneTorchDelegate();
    public RefillOneTorchDelegate OnRefillOneTorch;

    public delegate void SisterCleansedDelegate();
    public SisterCleansedDelegate OnSisterCleansed;

    public delegate void StoryEndingDelegate();
    public StoryEndingDelegate StartEnd;

    private void Awake()
    {
        hasTakenFogDamage = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartEnd?.Invoke();
        }
    }
}
