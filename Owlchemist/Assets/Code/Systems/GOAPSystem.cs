using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GOAPSystem : BaseSystem
{
    public Filter[] filters;

    private GOAPPlanner planner;
    private WorldStateComponent worldStateComponent;

    KeyValuePair<string, object> playerLitState = new KeyValuePair<string, object>("playerLit", true);
    //public HashSet<KeyValuePair<string, object>> currentWorldState;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(int id,
            GameObject go,
            EnemyComponent ec,
            CombatComponent cc,
            HealthComponent hc,
            GOAPComponent aic
            )
        {
            this.id = id;

            gameObject = go;
            enemyComponent = ec;
            combatComponent = cc;
            healthComponent = hc;
            aiComponent = aic;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public EnemyComponent enemyComponent;
        public CombatComponent combatComponent;
        public HealthComponent healthComponent;
        public GOAPComponent aiComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        worldStateComponent = GetComponentInChildren<WorldStateComponent>();

        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            EnemyComponent ec = objects[i].GetComponent<EnemyComponent>();
            CombatComponent cc = objects[i].GetComponent<CombatComponent>();
            HealthComponent hc = objects[i].GetComponent<HealthComponent>();
            GOAPComponent aic = objects[i].GetComponent<GOAPComponent>();

            if (cc && hc && ec && aic)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ec, cc, hc, aic));

                aic.stateMachine = new FSM();
                aic.availableActions = new HashSet<GOAPAction>();
                aic.currentActions = new Queue<GOAPAction>();

                GOAPAction[] actions = aic.gameObject.GetComponents<GOAPAction>();
                foreach (GOAPAction action in actions)
                {
                    aic.availableActions.Add(action);
                    action.constantTarget = worldStateComponent.gameObject;
                }

                CreateIdleState(aic);
                CreateMoveToState(aic);
                CreatePerformActionState(aic);
                aic.stateMachine.pushState(aic.idleState);
            }
        }

        filters = tmpFilters.ToArray();

        planner = new GOAPPlanner();
        //currentWorldState = new HashSet<KeyValuePair<string, object>>();

        KeyValuePair<string, object> playerDmgState = new KeyValuePair<string, object>("damagePlayer", false);
        KeyValuePair<string, object> playerStalkState = new KeyValuePair<string, object>("stalkPlayer", false);

        worldStateComponent.worldState.Add(playerLitState);
        worldStateComponent.worldState.Add(playerDmgState);
        worldStateComponent.worldState.Add(playerStalkState);

        /*currentWorldState.Add(playerLitState);
        currentWorldState.Add(playerDmgState);
        currentWorldState.Add(playerStalkState);*/
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        inputComponent.OnAButtonDown += ToggleLightState;
    }

    public override void Tick(float deltaTime)
    {
        //ToggleLightState();

        for (int i = 0; i < filters.Length; i++)
        {
            Filter filter = filters[i];

            EnemyComponent enemyComp = filter.enemyComponent;
            CombatComponent combComp = filter.combatComponent;
            HealthComponent healthComp = filter.healthComponent;
            GOAPComponent aiComp = filter.aiComponent;

            aiComp.stateMachine.Update(aiComp.gameObject);

            if (!aiComp.HasMaxEnergy())
            {
                aiComp.RegenerateEnergy(deltaTime);
            }
        }
    }

    public void ToggleLightState()
    {
        if (worldStateComponent.worldState.Contains(playerLitState))
        {
            worldStateComponent.worldState.Remove(playerLitState);
            playerLitState = new KeyValuePair<string, object>(playerLitState.Key, !(bool)playerLitState.Value);
            worldStateComponent.worldState.Add(playerLitState);
        }
    }

    public void SetLightState(bool value)
    {
        if (worldStateComponent.worldState.Contains(playerLitState))
        {
            worldStateComponent.worldState.Remove(playerLitState);
            playerLitState = new KeyValuePair<string, object>(playerLitState.Key, value);
            worldStateComponent.worldState.Add(playerLitState);
        }
    }

    private void CreateIdleState(GOAPComponent aic)
    {
        aic.idleState = (fsm, obj) =>
        {
            //aic.AnnounceIdleState();

            HashSet<KeyValuePair<string, object>> worldState = worldStateComponent.worldState;
            HashSet<KeyValuePair<string, object>> goal = aic.CreateGoalState();

            Queue<GOAPAction> plan = planner.plan(aic.gameObject, aic.availableActions, worldState, goal);
            /*int index = 1;
            foreach (GOAPAction a in aic.availableActions)
            {
                Debug.Log("action no: " + index + " " + a);
                index++;
            }*/

            if (plan != null)
            {
                aic.currentActions = plan;
                aic.PlanFound(goal, plan);

                fsm.popState();
                fsm.pushState(aic.performActionState);
            }
            else
            {
                aic.PlanFailed(goal);
                fsm.popState();
                fsm.pushState(aic.idleState);
            }
        };
    }

    private void CreateMoveToState(GOAPComponent aic)
    {
        aic.moveToState = (fsm, gameObject) =>
        {
            //aic.AnnounceMoveToState();

            GOAPAction action = aic.currentActions.Peek();
            if (action.requiresInRange() && action.target == null)
            {
                fsm.popState();
                fsm.popState();
                fsm.pushState(aic.idleState);
                return;
            }

            if (aic.IsAgentInRange(action))
            {
                fsm.popState();
            }
            else
            {
                if (aic.IsActionInterrupted(action))
                {
                    fsm.popState();
                    fsm.pushState(aic.idleState);
                }
                aic.gameObject.GetComponent<NavMeshAgent>()?.SetDestination(action.target.transform.position);
            }
        };
    }

    private void CreatePerformActionState(GOAPComponent aic)
    {
        aic.performActionState = (fsm, obj) =>
        {
            //aic.AnnouncePerformActionState();

            if (!aic.HasActionPlan())
            {
                fsm.popState();
                fsm.pushState(aic.idleState);
                aic.ActionsFinished();
                return;
            }

            GOAPAction action = aic.currentActions.Peek();
            if (action.isDone())
            {
                aic.currentActions.Dequeue();
            }

            if (aic.HasActionPlan())
            {
                action = aic.currentActions.Peek();
                bool inRange = action.requiresInRange() ? action.isInRange() : true;

                if (inRange)
                {
                    bool success = action.perform(obj);
                    if (!success)
                    {
                        fsm.popState();
                        fsm.pushState(aic.idleState);
                        CreateIdleState(aic);
                        aic.PlanAborted(action);
                    }
                }
                else
                {
                    fsm.pushState(aic.moveToState);
                }
            }
            else
            {
                fsm.popState();
                fsm.pushState(aic.idleState);
                aic.ActionsFinished();
            }
        };
    }
}
