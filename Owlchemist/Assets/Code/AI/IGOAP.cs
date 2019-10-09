using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGOAP {

	HashSet<KeyValuePair<string, object>> GetWorldState();

	HashSet<KeyValuePair<string, object>> CreateGoalState();

	void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

	void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions);

	void ActionsFinished();

	void PlanAborted(GOAPAction aborter);

	bool IsAgentInRange(GOAPAction nextAction);

    bool IsActionInterrupted(GOAPAction action);
}
