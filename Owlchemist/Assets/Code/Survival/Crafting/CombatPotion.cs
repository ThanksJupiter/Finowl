using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPotion : MonoBehaviour
{
    public enum BattlePoition
    {
        LIGHT,
        QUEST,
        HEALTH
    }
    float intensity;
    public GameObject player;
    public BattlePoition BattleType = BattlePoition.LIGHT;
    public bool isCurrentlyActivated = false;
    public bool questComplete = false;
    private bool potionDelay = true;
    private float potionLightDelay = 0.7f;
    public GameObject healthIndicator;
    public GameObject questObjective;
    public GameObject ChildVFXQuestComplete;
    public Vector3 originalPos;
    public GameObject ChildVFXQuestMissed;
    public GameObject storyActivated;
    public GameObject wellSmoke;

    private void Start()
    {
       // originalPos = ChildVFXQuestComplete.transform.position;//transform.GetChild(0).transform.position;//transform.position;

        if (BattleType == BattlePoition.LIGHT)
        {
            intensity = GetComponentInChildren<Light>().intensity;
            GetComponentInChildren<Light>().enabled = false;

        }
    }
    void Update()
    {
        if (BattleType == BattlePoition.LIGHT)
        {
            if (!potionDelay)
            {
                isCurrentlyActivated = true;
                GetComponentInChildren<Light>().intensity--;

                if (GetComponentInChildren<Light>().intensity <= 2)
                {
                    gameObject.SetActive(false);
                    GetComponentInChildren<Light>().intensity = intensity;
                    isCurrentlyActivated = false;
                    potionDelay = true;
                    GetComponentInChildren<Light>().enabled = false;

                }
            }
            else
            {
                potionLightDelay -= Time.deltaTime;
                if(potionLightDelay < 0)
                {
                    potionDelay = false;
                    potionLightDelay = 0.7f;
                    GetComponentInChildren<Light>().enabled = true;
                }
            }
        }
        if (BattleType == BattlePoition.QUEST)
        {
            /*
            isCurrentlyActivated = true;
  
             if (Vector3.Distance(questObjective.transform.position, transform.position) < 5 && questComplete == false)
             {
                if (storyActivated.GetComponent<SisterDelete>())
                {
                    Debug.Log("Hangon!!");
                    player.GetComponent<PlayerEventComponent>().OnSisterCleansed();
                }
                if (storyActivated.GetComponent<StoryEndGame>())
                {
                    Debug.Log("grönaluyctan");
                    player.GetComponent<PlayerEventComponent>().StartEnd();
                }
                 questComplete = true;                 
            
             }
             */
            if (questComplete)
            {
                

                ChildVFXQuestComplete.SetActive(true);
                if (ChildVFXQuestComplete.GetComponent<ParticleSystem>().isStopped) //(GetComponentInChildren<Light>().intensity <= 2)
                {
                    isCurrentlyActivated = false;
                    ChildVFXQuestComplete.SetActive(false);
                    gameObject.SetActive(false);

                    if (wellSmoke != null && wellSmoke.activeSelf)
                    {
                        wellSmoke.SetActive(false);
                    }
                }
            }
            else
            {
                ChildVFXQuestMissed.SetActive(true);
                if (ChildVFXQuestMissed.GetComponent<ParticleSystem>().isStopped) //(GetComponentInChildren<Light>().intensity <= 2)
                {
                    isCurrentlyActivated = false;
                    ChildVFXQuestMissed.SetActive(false);
                    gameObject.SetActive(false);

                }
            }
            


        }
        if(BattleType == BattlePoition.HEALTH)
        {
            isCurrentlyActivated = true;
                if (transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().isStopped) //(GetComponentInChildren<Light>().intensity <= 2)
                {
                    isCurrentlyActivated = false;
                    //healthIndicator.GetComponent<HealthIndicator>().RestoreHeart();
                    gameObject.SetActive(false);
                }
            
        }

    }
}
