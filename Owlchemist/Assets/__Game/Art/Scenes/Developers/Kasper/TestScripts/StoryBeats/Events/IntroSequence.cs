using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "StoryEvent/IntroSequence")]
public class IntroSequence : BaseStoryEvent
{
    public string[] introTexts;

    private Text introText;
    private int currentText = 0;

    private PlayerFilter player;
    private GameObject canvasObj;
    private CanvasGroupFader tp;

    public override void TriggerStoryBegin(PlayerFilter playerFilter, GameObject obj)
    {
        canvasObj = obj;
        currentText = 0;
        player = playerFilter;
        introText = obj.GetComponentInChildren<Text>();
        introText.text = introTexts[currentText];

        player.gameManagerComponent.OnStopGameTick?.Invoke();

        player.inputComponent.OnAButtonDown += UpdateText;
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

    private void UpdateText()
    {
        currentText++;

        if (currentText >= introTexts.Length)
        {
            player.gameManagerComponent.OnStartGameTick?.Invoke();
            player.inputComponent.OnAButtonDown -= UpdateText;
            canvasObj.SetActive(false);
            //player.playerStoryComponent.storyBeats[player.playerStoryComponent.currentBeat].isDone = true;
            player.playerStoryComponent.currentStoryCollection.isStoryBeatDone = true;
            player.playerStoryComponent.introSequenceDone = true;
            return;
        }

        introText.text = introTexts[currentText];
    }

    public override void TriggerStoryEvent(PlayerFilter playerFilter, GameObject obj)
    {
        canvasObj = obj;
        currentText = 0;
        player = playerFilter;
        introText = obj.GetComponentInChildren<Text>();
        introText.text = introTexts[currentText];

        player.gameManagerComponent.OnStopGameTick?.Invoke();

        player.inputComponent.OnAButtonDown += UpdateText;
    }
}
