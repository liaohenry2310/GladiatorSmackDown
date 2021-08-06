using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GladiatorGame
{
    public class CombatState : GameState
    {
        Gladiator[] _gladiators;
        NavigationGraph _navigationGraph;

        public override string GetName()
        {
            return "Combat";
        }

        public override void OnEnter()
        {
            gameObject.SetActive(true);

            _navigationGraph = GetComponentInChildren<NavigationGraph>();
            _gladiators = GetComponentsInChildren<Gladiator>();

            _navigationGraph.Initialize();

            foreach (var g in _gladiators)
                g.Initialize();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ChangeState("Postcombat");

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 desination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2[] path = _navigationGraph.GetPath(_gladiators[0].transform.position.ToVector2(), desination);
                _gladiators[0].SetPath(path);
            }
            if (Input.GetMouseButtonDown(1))
            {
                _gladiators[0].CancelPath();
            }

            foreach (var g in _gladiators)
                g.OnUpdate();
        }

        public override void OnExit()
        {
            gameObject.SetActive(false);
        }
    }
}