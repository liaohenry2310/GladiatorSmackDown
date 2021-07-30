using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GladiatorGame
{
    public class PostcombatState : GameState
    {
        public override string GetName()
        {
            return "Postcombat";
        }

        public override void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ChangeState("MainMenu");
        }

        public override void OnExit()
        {
            gameObject.SetActive(false);
        }
    }
}