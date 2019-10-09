public class PlayerStoryComponent : BaseComponent
{
    public int currentBeat { get; set; }
    public bool introSequenceDone { get; set; }

    public StoryBeatComponent currentStoryBeat;
    //public StoryBeatComponent[] storyBeats;

    public StoryBeatComponent.StoryBeatCollection currentStoryCollection;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        currentBeat = 0;
        introSequenceDone = false;
    }
}
