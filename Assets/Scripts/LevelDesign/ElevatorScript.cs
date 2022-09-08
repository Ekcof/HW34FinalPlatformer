using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private Transform upperLimit;
    [SerializeField] private Transform lowerLimit;
    [SerializeField] private float stopDistance;
    [SerializeField] private float speed;
    [SerializeField] private bool isGoingUp;
    [SerializeField] private float delay;
    private bool move;
    private float currentDelay;
    private Rigidbody2D rigidBody;
    private bool isPressed;
    private Animator animator;
    private float distance;

    private void Awake()
    {
        currentDelay = delay;
        animator = lever.GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        isPressed = animator.GetBool("Pressed");
    }

    private void Update()
    {
        if (isPressed != animator.GetBool("Pressed"))
        {
            isPressed = !isPressed;
            isGoingUp = !isGoingUp;
            currentDelay = delay;
            move = true;
        }
        if (move)
        {
            if (currentDelay <= 0)
            {
                MovePlatform(isGoingUp);
            }
        }
        if (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
        }
    }


    private void MovePlatform(bool whereToGo)
    {
        Transform limit = null;
        if (whereToGo) { limit = upperLimit; } else { limit = lowerLimit; }
        distance = Vector2.Distance(limit.position, transform.position);
        if (distance >= stopDistance)
        {
            move = true;
            Vector2 direction = transform.position - limit.position;
            rigidBody.velocity = direction.normalized * speed * -1;
        }
        else
        {
            StopPlatform();
        }
    }

    private void StopPlatform()
    {
        move = false;
        rigidBody.velocity = new Vector2(0f, 0f);
    }
}
