using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class GOAPAgent : MonoBehaviour {

	private FSM stateMachine;
	private FSM.FSMState idleState;
	private FSM.FSMState moveToState;
	private FSM.FSMState performActionState;

	private HashSet<GOAPAction> availableActions;
	private Queue<GOAPAction> currentActions;
	private IGOAP dataProvider;
	private GOAPPlanner planner;


	// Use this for initialization
	void Start () {
		stateMachine = new FSM ();
		availableActions = new HashSet<GOAPAction> ();
		currentActions = new Queue<GOAPAction> ();
		planner = new GOAPPlanner ();
		findDataProvider ();
		createIdleState ();
		createMoveToState ();
		createPerformActionState ();
		stateMachine.pushState (idleState);
		loadActions ();
	}
	
	// Update is called once per frame
	void Update () {
		stateMachine.Update (this.gameObject);
	}

	public void addAction(GOAPAction action){
		availableActions.Add (action);
	}

	public GOAPAction getAction(Type action){
		foreach (GOAPAction currAction in availableActions) {
			if(currAction.GetType().Equals(action)){
				return currAction;
			}
		}

		return null;
	}

	public void removeAction(GOAPAction action){
		availableActions.Remove (action);
	}

	private bool hasActionPlan(){
		return currentActions.Count > 0;
	}

    // private FSM.FSMState
	private void createIdleState(){
		idleState = (fsm, obj) => {
			HashSet<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState ();
			HashSet<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState ();

			Queue<GOAPAction> plan = planner.plan (gameObject, availableActions, worldState, goal);
			if (plan != null) {
				currentActions = plan;
				dataProvider.PlanFound (goal, plan);

				fsm.popState ();
				fsm.pushState (performActionState);
			} else {
				dataProvider.PlanFailed (goal);
				fsm.popState ();
				fsm.pushState (idleState);
			}
		};
	}

	private void createMoveToState(){
		moveToState = (fsm, gameObject) => {

			GOAPAction action = currentActions.Peek ();
			if (action.requiresInRange () && action.target == null) {
				fsm.popState ();
				fsm.popState ();
				fsm.pushState (idleState);
				return;
			}

			if (dataProvider.IsAgentInRange(action))
            {
				fsm.popState ();
			}
		};
	}

	private void createPerformActionState(){

		performActionState = (fsm, obj) => {

			if (!hasActionPlan()) {
				fsm.popState ();
				fsm.pushState (idleState);
				dataProvider.ActionsFinished ();
				return;
			}

			GOAPAction action = currentActions.Peek ();
			if (action.isDone ()) {
				currentActions.Dequeue ();
			}

			if (hasActionPlan ()) {
				action = currentActions.Peek ();
				bool inRange = action.requiresInRange () ? action.isInRange () : true;

				if (inRange) {
					bool success = action.perform (obj);
					if (!success) {
						fsm.popState ();
						fsm.pushState(idleState);
						createIdleState();
						dataProvider.PlanAborted (action);
					} 
				} else {
					fsm.pushState (moveToState);
				}
			} else {
				fsm.popState ();
				fsm.pushState (idleState);
				dataProvider.ActionsFinished ();
			}
		};
	}

	private void findDataProvider(){
		foreach (Component comp in gameObject.GetComponents(typeof(Component))) {
			if (typeof(IGOAP).IsAssignableFrom (comp.GetType ())) {
				dataProvider = (IGOAP)comp;
				return;
			}
		}
	}

	private void loadActions(){
		GOAPAction[] actions = gameObject.GetComponents<GOAPAction> ();
		foreach (GOAPAction a in actions) {
			availableActions.Add (a);
		}
	}
}
