using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    protected AIStateMachine stateMachine;
    protected GameObject agent;

    public AIState(AIStateMachine machine)
    {
        stateMachine = machine;
        agent = machine.gameObject;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}