using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIState
{
    private Perception agentPerception;
    private Movement agentMovement;
    private Pathfinder agentPathfinder;

    public AIChaseState(AIStateMachine machine) : base(machine) { }

    public override void Enter()
    {
        Debug.Log(agent.name + " has entered chase state!");
        agentPerception = agent.GetComponent<Perception>();
        agentMovement = agent.GetComponent<Movement>();
        agentPathfinder = agent.GetComponent<Pathfinder>();
    }

    public override void Update()
    {
        GameObject enemy = agentPerception.LookForEnemy();
        if (enemy != null)
        {
            Vector2 enemyPosition = enemy.transform.position;
            Vector2 agentPosition = agent.transform.position;

            agentMovement.SetPathToFollow(agentPathfinder.GetPath(enemyPosition));

            if (Vector2.Distance(enemyPosition, agentPosition) <= 1.0f)
            {
                stateMachine.ChangeState(new AICombatState(stateMachine));
            }
        }
        else
        {
            stateMachine.ChangeState(new AIWanderState(stateMachine));
        }
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {

    }
}