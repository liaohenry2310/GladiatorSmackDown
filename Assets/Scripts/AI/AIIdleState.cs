using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public AIIdleState(AIStateMachine machine) : base(machine) { }

    public override void Enter()
    {
        Debug.Log(agent.name + " has entered idle state!");
        agent.GetComponent<Movement>().StopMoving();
    }

    public override void Update()
    {
        stateMachine.ChangeState(new AIWanderState(stateMachine));
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {

    }
}