using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GladiatorGame
{
    public abstract class GameState : MonoBehaviour
    {
        protected void ChangeState(string stateName)
        {
            MainGame.Instance.ChangeState(stateName);
        }

        public abstract string GetName();
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}