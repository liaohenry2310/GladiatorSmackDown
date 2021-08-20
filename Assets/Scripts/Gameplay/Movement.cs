using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    public UnityEvent onStartMove;
    public UnityEvent onStopMove;

    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float stopDistance = 1.0f;

    private List<Vector2> path;
    private int pathIndex = 0;
    private bool isMoving = false;

    public bool IsCurrentlyMoving()
    {
        return isMoving;
    }

    public void SetPathToFollow(List<Vector2> newPath)
    {
        path = newPath;
        isMoving = true;
        pathIndex = 0;
        animator.SetBool("IsMoving", true);
        onStartMove.Invoke();
    }

    public void StopMoving()
    {
        isMoving = false;
        animator.SetBool("IsMoving", false);
        onStopMove.Invoke();
    }

    private void Update()
    {
        //Ensures correct sprite rendering order via z-axis position.
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        //Ensures correct sprite facing direction.
        if (isMoving)
        {
            if (rigidbody.velocity.x <= 0.0f)
                animator.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else
                animator.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving && pathIndex < path.Count)
        {
            Vector2 desiredPosition = path[pathIndex];
            Vector2 currentPosition = rigidbody.position;
            Vector2 desiredVelocity = (desiredPosition - currentPosition).normalized * movementSpeed;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, desiredVelocity, acceleration * Time.fixedDeltaTime);

            if (Vector2.Distance(desiredPosition, currentPosition) <= stopDistance)
            {
                ++pathIndex;
            }
        }
        else
        {
            StopMoving();
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, Vector2.zero, acceleration * Time.fixedDeltaTime);
        }
    }
}