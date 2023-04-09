using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float patrolSpeed = 2f; // speed when patrolling
    public float chaseSpeed = 4f; // speed when chasing the player
    public float chaseRadius = 5f; // radius at which the enemy will start chasing the player
    public Transform[] waypoints; // array of waypoints to patrol between

    private int currentWaypointIndex = 0; // index of the current waypoint
    private Transform target; // the target (either the player or a waypoint)
    private Rigidbody2D rb; // reference to the enemy's rigidbody2D component

    void Start()
    {
        // get reference to the enemy's rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // set the target to the first waypoint
        target = waypoints[currentWaypointIndex];
    }

    void Update()
    {
        // get distance to the target
        float distance = Vector2.Distance(transform.position, target.position);

        // if the target is the player
        if (target.tag == "Player")
        {
            // if the player is within the chase radius
            if (distance < chaseRadius)
            {
                // move towards the player
                rb.velocity = (target.position - transform.position).normalized * chaseSpeed;
            }
            // if the player is outside the chase radius
            else
            {
                // go back to patrolling
                rb.velocity = Vector2.zero;
                currentWaypointIndex = 0;
                target = waypoints[currentWaypointIndex];
            }
        }
        // if the target is a waypoint
        else
        {
            // if the enemy has reached the waypoint
            if (distance < 0.1f)
            {
                // go to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                target = waypoints[currentWaypointIndex];
            }
            // if the enemy has not reached the waypoint
            else
            {
                // move towards the waypoint
                rb.velocity = (target.position - transform.position).normalized * patrolSpeed;
            }
        }
    }
}