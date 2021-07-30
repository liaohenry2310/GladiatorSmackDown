using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GladiatorGame
{
    public class MainGame : MonoBehaviour
    {
        static public MainGame Instance { get { return _instance; } }
        static private MainGame _instance = null;

        [SerializeField] GameState[] gameStates;
        [SerializeField] int currentState = 0;

        public void ChangeState(string stateName)
        {
            int prevState = currentState;
            for (int i = 0; i < gameStates.Length; ++i)
            {
                if (gameStates[i].GetName() == stateName)
                {
                    gameStates[prevState].OnExit();
                    currentState = i;
                    gameStates[i].OnEnter();
                    return;
                }
            }
        }

        private MainGame() { }

        private void Awake()
        {
            if (!_instance)
                _instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            gameStates[currentState].OnEnter();
        }

        private void Update()
        {
            gameStates[currentState].OnUpdate();
        }
    }
}


