using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class TutorialBehavior : MonoBehaviour
{
    public GameObject navPoint;

    [SerializeField]
    public NavMeshAgent agent;

    public GameObject triggerBox;

    private void Awake()
    {
        agent.GetComponent<NavMeshAgent>();
        Assert.IsNotNull(agent, "Failed to locate <b>NavMeshAgent</b> component on <b>TutorialEnemy</b>");

        triggerBox = GameObject.FindGameObjectWithTag("PlayerSensor");
        Assert.IsNotNull(triggerBox, "Failed to locate <b>TriggerBox</b> game object");

        navPoint = GameObject.FindGameObjectWithTag("NavPoint");
    }

    private void Update()
    {
        if (triggerBox.GetComponent<TutorialOnSense>().enter)
        {
            agent.SetDestination(navPoint.transform.position);
            Debug.Log(agent.remainingDistance + " remaing distance for <b>TUTORIAL AI</b>");
            if (agent.remainingDistance > 0 && agent.remainingDistance <= 0.5f)
            {
                Debug.Log(agent.remainingDistance + " remaing distance for <b>TUTORIAL AI</b>");
                agent.gameObject.SetActive(false);
            }
        }
    }
}
