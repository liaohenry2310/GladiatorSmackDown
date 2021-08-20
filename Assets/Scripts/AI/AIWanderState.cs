using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIWanderState : AIState
{
    private Perception agentPerception;
    private Movement agentMovement;
    private Pathfinder agentPathfinder;

    private int minDistPerPath = 2;
    private int maxDistPerPath = 4;

    public AIWanderState(AIStateMachine machine) : base(machine) { }

    public override void Enter()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        Debug.Log(agent.name + " has entered wander state!");
        agentPerception = agent.GetComponent<Perception>();
        agentMovement = agent.GetComponent<Movement>();
        agentPathfinder = agent.GetComponent<Pathfinder>();
    }

    public override void Update()
    {
        if (!agentMovement.IsCurrentlyMoving())
        {
            List<Vector2> availablePositions = new List<Vector2>();
            for (int x = -maxDistPerPath; x < maxDistPerPath; ++x)
            {
                if (x > -minDistPerPath && x < minDistPerPath)
                    x = minDistPerPath;

                for (int y = -maxDistPerPath; y < maxDistPerPath; ++y)
                {
                    if (y > -minDistPerPath && y < minDistPerPath)
                        y = minDistPerPath;

                    Vector2 position = new Vector2(x, y);
                    if (agentPathfinder.isPositionValid(position))
                        availablePositions.Add(position);
                }
            }

            agentMovement.SetPathToFollow(agentPathfinder.GetPath(availablePositions[Random.Range(0, availablePositions.Count - 1)]));
        }

        if (agentPerception.LookForEnemy() != null)
        {
            stateMachine.ChangeState(new AIChaseState(stateMachine));
        }
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {

    }
}