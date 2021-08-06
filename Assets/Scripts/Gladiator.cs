using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GladiatorGame
{
    public class Gladiator : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float acceleration;
        [SerializeField] float stoppingDistance;
        [SerializeField] Animator animator;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] LineRenderer debugPath;

        private Queue<Vector2> _path = new Queue<Vector2>();
        private Vector2 _previousPoint;
        private bool _isMoving = false;

        public void Initialize()
        {

        }

        public void OnUpdate()
        {
            if (debugPath.gameObject.activeInHierarchy)
            {
                List<Vector3> vertices = new List<Vector3>();
                vertices.Add(_previousPoint);
                vertices.AddRange(_path.ToArray().ToVector3());
                debugPath.positionCount = vertices.Count;
                debugPath.SetPositions(vertices.ToArray());
            }

            if (_isMoving && _path.Count > 0)
            {
                animator?.SetBool("IsMoving", true);
                Vector2 currentPosition = transform.position.ToVector2();
                Vector2 desiredPosition = _path.Peek();
                Vector2 desiredVelocity = (desiredPosition - currentPosition);
                desiredVelocity.Normalize();
                desiredVelocity *= moveSpeed;
                rb.velocity = Vector2.Lerp(rb.velocity, desiredVelocity, acceleration * Time.deltaTime);

                if (rb.velocity.x <= 0.0f)
                    animator.gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else
                    animator.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                if (Vector2.Distance(desiredPosition, currentPosition) <= stoppingDistance)
                {
                    _previousPoint = _path.Dequeue();
                }
            }
            else
            {
                animator?.SetBool("IsMoving", false);
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, acceleration * Time.deltaTime);
            }
        }

        //public void MoveTo(Vector2 destination)
        //{
        //    _isMoving = true;
        //    _path.Enqueue(destination);
        //}

        public void SetPath(Vector2[] path)
        {
            if (path.Length == 0)
                return;

            CancelPath();
            _isMoving = true;
            foreach (var v in path)
            {
                _path.Enqueue(v);
            }
            _previousPoint = _path.Peek();
        }

        public void CancelPath()
        {
            _isMoving = false;
            _path.Clear();
        }
    }
}


