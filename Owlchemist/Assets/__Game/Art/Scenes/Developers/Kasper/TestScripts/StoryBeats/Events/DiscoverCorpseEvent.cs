using UnityEngine;

[CreateAssetMenu(menuName = "StoryEvent/DiscoverTorchEvent")]
public class DiscoverCorpseEvent : BaseStoryEvent
{
    private GameObject activeCanvas;
    private CanvasGroupFader tp;

    public override void TriggerStoryBegin(PlayerFilter playerFilter, GameObject obj)
    {
        Light l = obj.GetComponentInChildren<Light>();
        ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem>();
        l.intensity = 1f;
        playerFilter.vfxComponent.discoverSisterTorchFizzle.Play();

        playerFilter.inputComponent.PulseVibrate(.3f);
    }

    public override void TriggerStoryProgress(PlayerFilter playerFilter, GameObject obj)
    {
        tp = obj.GetComponent<CanvasGroupFader>();
        tp?.Display(false);
    }

    public override void TriggerStoryEnd(PlayerFilter playerFilter, GameObject obj)
    {
        tp?.Hide(false);
    }

    public override void TriggerStoryEvent(PlayerFilter playerFilter, GameObject obj)
    {
        Light l = obj.GetComponentInChildren<Light>();
        ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem>();
        l.intensity = 1f;
        playerFilter.vfxComponent.discoverSisterTorchFizzle.Play();

        playerFilter.inputComponent.PulseVibrate(.3f);
    }
}
