using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GladiatorGame
{
    public class PrecombatState : GameState
    {
        public override string GetName()
        {
            return "Precombat";
        }

        public override void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ChangeState("Combat");
        }

        public override void OnExit()
        {
            gameObject.SetActive(false);
        }
    }
}
