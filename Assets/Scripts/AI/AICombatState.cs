using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombatState : AIState
{
    public AICombatState(AIStateMachine machine) : base(machine) { }

    public override void Enter()
    {
        Debug.Log(agent.name + " has entered combat state!");
        agent.GetComponent<Movement>().StopMoving();
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {

    }
}