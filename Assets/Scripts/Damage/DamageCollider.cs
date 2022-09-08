using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float force;
    private GameObject damagedObject;
    private Health health;
    private Rigidbody2D rigidBody2D;
    private Nameofthegame.Inputs.PlayerMovement playerMovement;
    private IEnumerator coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damagedObject = collision.gameObject;
        rigidBody2D = damagedObject.GetComponent<Rigidbody2D>();
        health = damagedObject.GetComponent<Health>();
        playerMovement = damagedObject.GetComponent<Nameofthegame.Inputs.PlayerMovement>();
        if (playerMovement == null)
        {
            playerMovement = damagedObject.GetComponentInParent<Nameofthegame.Inputs.PlayerMovement>();
        }
        if (playerMovement != null)
        {
            coroutine = LoseAndGetControl(0.2f, playerMovement, damagedObject);
            StartCoroutine(coroutine);
            //LoseAndGetControl(1f, playerMovement, damagedObject);
        }
        if (health == null)
        {
            health = damagedObject.GetComponentInParent<Health>();
        }
        if (health != null)
        {
            health.SetDamage(minDamage, maxDamage);
            if (rigidBody2D == null)
            {
                rigidBody2D = damagedObject.GetComponentInParent<Rigidbody2D>();
                if (rigidBody2D != null)
                {
                    Vector2 forceVector = transform.position - damagedObject.transform.position;
                    forceVector.Normalize();
                    forceVector = -forceVector * force;
                    if (forceVector.y < 0) { forceVector.y = 0.1f; };
                    rigidBody2D.AddForce(forceVector, ForceMode2D.Impulse);
                }
            }
        }
    }
 
    private IEnumerator LoseAndGetControl(float delay, Nameofthegame.Inputs.PlayerMovement playerMovement, GameObject damagedObject)
    {
        playerMovement.LoseControl();
        Animator animator = damagedObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = damagedObject.GetComponentInParent<Animator>();
        }
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(delay);
        playerMovement.ReturnControl();
        animator.SetBool("Hit", false);
    }
}
