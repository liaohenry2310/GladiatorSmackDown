using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GladiatorGame
{
    public class CombatState : GameState
    {
        public override string GetName()
        {
            return "Combat";
        }

        public override void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ChangeState("Postcombat");
        }

        public override void OnExit()
        {
            gameObject.SetActive(false);
        }
    }
}