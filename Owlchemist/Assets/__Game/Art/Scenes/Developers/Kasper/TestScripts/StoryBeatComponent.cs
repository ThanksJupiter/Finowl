using UnityEngine;

public class StoryBeatComponent : BaseComponent
{
    [Header("Settings")]
    public BaseStoryEvent storyEvent;
    public OwlStoryBeatProgression currentBeatProgression;

    public float firstTriggerThreshold;

    [Header("Game objects to modify on event triggers")]
    public GameObject begunTriggerObject;
    public GameObject progressTriggerObject;
    public GameObject endTriggerObject;

    [Header("Progress conditions")]
    public BaseStoryProgressionCondition beginCondition;
    public BaseStoryProgressionCondition progressCondition;
    public BaseStoryProgressionCondition endCondition;

    public bool storyBeginningDone { get; set; }
    public bool storyProgressDone { get; set; }
    public bool storyEndDone { get; set; }

    [Header("Prototype Extras")]
    public CollectibleComponent collectible;
    public GameObject endScreenObj;

    [Header("Remake Tests")]
    public StoryBeatCollection[] beatCollections;
    public int currentStoryBeat { get; set; }
    public bool isDone { get; set; }
    public float currentDelay { get; set; }
    public bool currentDelaySet { get; set; }
    public float currentWaitTime { get; set; }

    public StoryBeatCollection GetCurrentBeatCollection()
    {
        //if (beatCollections[currentStoryBeat] == null) { return null; }
        return beatCollections[currentStoryBeat];
    }

    [System.Serializable]
    public class StoryBeatCollection
    {
        [Header("Base Settings")]
        public GameObject storyBeatObject;
        public BaseStoryProgressionCondition enterBeatCondition;
        public BaseStoryProgressionCondition exitBeatCondition;
        public BaseStoryEvent storyEvent;
        [Header("Extras")]
        public float beginCheckDelay;
        public bool pausePlay;
        public Transform positionToCheck;
        [Header("Auxiliary CanvasGroupFader")]
        public CanvasGroupFader auxFader;
        public bool auxFadeOnBegin;
        [Header("Stats")]
        public bool isStoryBeatDone;
        public OwlStoryBeatProgression progression;
    }

    private void Awake()
    {
        storyBeginningDone = false;
        storyProgressDone = false;
        storyEndDone = false;
        currentStoryBeat = 0;
        currentWaitTime = 0f;
        currentDelaySet = false;
    }

    public bool CurrentDelaySet()
    {
        if (beatCollections[currentStoryBeat].beginCheckDelay == 0)
        {
            return false;
        }

        return currentDelaySet;
    }

    public void SetCurrentDelay()
    {
        currentDelay = beatCollections[currentStoryBeat].beginCheckDelay;
        currentDelaySet = true;
    }

    public bool WaitDelay()
    {
        currentWaitTime += Time.deltaTime;

        if (currentWaitTime >= currentDelay)
        {
            //Debug.Log("cur: " + currentWaitTime + " delay: " + currentDelay);
            ResetDelay();
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ResetDelay()
    {
        currentDelaySet = false;
        currentWaitTime = 0f;
        currentDelay = 0f;
    }

    public void TriggerStoryBeat(int index, PlayerFilter player)
    {
        if (beatCollections[index - 1] != null)
        {
            beatCollections[index - 1].storyEvent.TriggerStoryEnd(player, beatCollections[index].storyBeatObject);
        }
        beatCollections[index].storyEvent.TriggerStoryEvent(player, beatCollections[index].storyBeatObject);
        currentStoryBeat++;
    }

    public void TriggerBeginStory(PlayerFilter player)
    {
        storyEvent?.TriggerStoryBegin(player, begunTriggerObject);
        IncrementStoryBeatProgression();
    }

    public void TriggerProgressStory(PlayerFilter player)
    {
        storyEvent?.TriggerStoryProgress(player, progressTriggerObject);
        IncrementStoryBeatProgression();
    }

    public void TriggerEndStory(PlayerFilter player)
    {
        storyEvent?.TriggerStoryEnd(player, endTriggerObject);
        IncrementStoryBeatProgression();
    }

    public void IncrementStoryBeatProgression()
    {
        currentBeatProgression++;
        //Debug.Log("[" + name + "]" + " story beat progression: " + currentBeatProgression);
    }

    public void Reset()
    {
        currentBeatProgression = OwlStoryBeatProgression.None;
    }
}

public enum OwlStoryBeatProgression
{
    None,
    InProgress,
    End
}
