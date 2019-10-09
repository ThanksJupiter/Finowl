using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBeatSystem : BaseSystem
{
    private Filter[] filters;
    //private StoryBeatComponent currentStoryBeat;

    public List<StoryBeatComponent> storyBeats = new List<StoryBeatComponent>();

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            StoryBeatComponent sbc
            )
        {
            this.id = id;

            gameObject = go;
            storyBeatComponent = sbc;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public StoryBeatComponent storyBeatComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            StoryBeatComponent sbc = objects[i].GetComponent<StoryBeatComponent>();

            if (sbc)
            {
                storyBeats.Add(sbc);

                tmpFilters.Add(new Filter(index, objects[i].gameObject, sbc));
            }
        }

        player.playerStoryComponent.currentStoryBeat = storyBeats[0];

        /*if (storyBeats.Count != 0)
        {
            ProceedToNextBeat(storyBeats[0]);
        }*/
        filters = tmpFilters.ToArray();
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        //inputComponent.OnAButtonDown += DebugAPressed;
    }

    public override void Tick(float deltaTime)
    {
        /*for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            StoryBeatComponent sbcComp = filter.storyBeatComponent;

            // ----- logic -----

        }*/
        //if (player.playerStoryComponent.currentStoryBeat == null) { return; }

        StoryBeatComponent sbc = player.playerStoryComponent.currentStoryBeat;
        if (player.playerStoryComponent.currentBeat >= sbc.beatCollections.Length) { return; }
        StoryBeatComponent.StoryBeatCollection sc = sbc.GetCurrentBeatCollection();
        int index = player.playerStoryComponent.currentBeat;

        if (sc.progression == OwlStoryBeatProgression.None)
        {
            if (!sbc.CurrentDelaySet())
            {
                sbc.SetCurrentDelay();
            }
            if (sbc.WaitDelay())
            {
                return;
            }
        }
        switch (sc.progression)
        {
            case OwlStoryBeatProgression.None:
                if (sc.enterBeatCondition == null) { return; }
                if (sc.enterBeatCondition.ConditionMet(player, storyBeats[0]) && sc.enterBeatCondition.IsInRange(player, sc.positionToCheck))
                {
                    if (sc.auxFader && sc.auxFadeOnBegin)
                    {
                        sc.storyEvent.TriggerStoryEvent(player, sc.storyBeatObject, sc.auxFader);
                    }
                    else
                    {
                        sc.storyEvent.TriggerStoryEvent(player, sc.storyBeatObject);
                    }

                    player.playerStoryComponent.currentStoryCollection = sc;
                    sc.progression++;
                    if (sc.pausePlay)
                    {
                        player.gameManagerComponent.OnStopGameTick();
                    }
                }
                else
                {
                    //Debug.Log("[StoryBeatSystem] distance object is not null but in range condition is not used");
                }
                break;
            case OwlStoryBeatProgression.InProgress:
                if (sc.exitBeatCondition == null) { return; }
                if (sc.exitBeatCondition.ConditionMet(player, storyBeats[0]) && sc.exitBeatCondition.IsInRange(player, sc.positionToCheck))
                {
                    if (sc.auxFader && !sc.auxFadeOnBegin)
                    {
                        sc.storyEvent.TriggerStoryEnd(player, sc.storyBeatObject, sc.auxFader);
                    }
                    else
                    {
                        sc.storyEvent.TriggerStoryEnd(player, sc.storyBeatObject);
                    }
                    sc.isStoryBeatDone = true;
                    sc.progression++;
                    if (sc.pausePlay)
                    {
                        player.gameManagerComponent.OnStartGameTick();
                    }
                }
                break;
            case OwlStoryBeatProgression.End:
                if (sc.isStoryBeatDone)
                {
                    TryProceedToNextBeat(index);
                }
                break;
            default:
                break;
            
        }

        /*switch (player.playerStoryComponent.currentStoryBeat.currentBeatProgression)
        {
            case OwlStoryBeatProgression.None:
                BeatBeginningCheck();
                break;
            case OwlStoryBeatProgression.Begun:
                BeatProgressCheck();
                break;
            case OwlStoryBeatProgression.InProgress:
                BeatEndCheck();
                break;
            case OwlStoryBeatProgression.End:
                TryProceedToNextBeat(player.playerStoryComponent.currentBeat);
                break;
            default:
                break;
        }*/
    }

    /*private void BeatBeginningCheck()
    {
        if (!player.playerStoryComponent.currentStoryBeat.beginCondition)
        {
            Debug.LogWarning("No condition to begin event, skipping to next event");
            TryProceedToNextBeat(player.playerStoryComponent.currentBeat);
            return;
        }

        if (player.playerStoryComponent.currentStoryBeat.beginCondition.ShouldProgress(player, player.playerStoryComponent.currentStoryBeat))
        {
            player.playerStoryComponent.currentStoryBeat.TriggerBeginStory(player);
        }
    }

    private void BeatProgressCheck()
    {
        if (!player.playerStoryComponent.currentStoryBeat.progressCondition)
        {
            Debug.LogWarning("No condition to progress event, skipping to next event");
            TryProceedToNextBeat(player.playerStoryComponent.currentBeat);
            return;
        }

        if (player.playerStoryComponent.currentStoryBeat.progressCondition.ShouldProgress(player, player.playerStoryComponent.currentStoryBeat))
        {
            player.playerStoryComponent.currentStoryBeat.TriggerProgressStory(player);
        }
    }

    private void BeatEndCheck()
    {
        if (!player.playerStoryComponent.currentStoryBeat.endCondition)
        {
            Debug.LogWarning("No condition to end event, skipping to next event");
            TryProceedToNextBeat(player.playerStoryComponent.currentBeat);
            return;
        }

        if (player.playerStoryComponent.currentStoryBeat.endCondition.ShouldProgress(player, player.playerStoryComponent.currentStoryBeat))
        {
            player.playerStoryComponent.currentStoryBeat.TriggerEndStory(player);
        }
    }*/

    private void TryProceedToNextBeat(int nextBeatIndex)
    {
        if (nextBeatIndex < player.playerStoryComponent.currentStoryBeat.beatCollections.Length)
        {
            ProceedToNextBeat(storyBeats[0]);
        }
        else
        {
            //player.playerStoryComponent.currentStoryBeat = null;
            Debug.LogWarning("No story beat at index: " + nextBeatIndex + " no story beat currently active");
        }
    }

    private void ProceedToNextBeat(StoryBeatComponent newBeat)
    {
        newBeat.currentStoryBeat++;
        if (newBeat.currentStoryBeat >= newBeat.beatCollections.Length)
        {
            storyBeats.Remove(newBeat);
        }

        //player.playerStoryComponent.currentStoryBeat = newBeat;
        player.playerStoryComponent.currentBeat++;
        //player.playerStoryComponent.currentStoryCollection = newBeat.beatCollections[newBeat.currentStoryBeat];
    }
}
