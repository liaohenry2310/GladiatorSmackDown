using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GladiatorGame
{
    public class MainMenuState : GameState
    {
        public override string GetName()
        {
            return "MainMenu";
        }

        public override void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ChangeState("Precombat");
        }

        public override void OnExit()
        {
            gameObject.SetActive(false);
        }
    }
}

