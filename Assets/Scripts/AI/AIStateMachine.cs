using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    AIState currentState = null;

    public void ChangeState(AIState newState)
    {
        if (!enabled)
            return;

        enabled = false;
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
        enabled = true;
    }

    private void Awake()
    {
        ChangeState(new AIIdleState(this));
    }

    private void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}
