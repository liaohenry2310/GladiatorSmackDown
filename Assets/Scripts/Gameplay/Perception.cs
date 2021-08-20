using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    [SerializeField] private float sightRange = 1.0f;

    public GameObject LookForEnemy()
    {
        Vector2 currentPosition = transform.position.ToVector2();
        Collider2D[] detected = Physics2D.OverlapCircleAll(currentPosition, sightRange, LayerMask.GetMask("Gladiators"));

        foreach(var d in detected)
        {
            if (d.gameObject != gameObject && !Physics2D.Linecast(currentPosition, d.transform.position.ToVector2(), LayerMask.GetMask("Walls")))
                return d.gameObject;
        }
        
        return null;
    }
}
