using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private string hitTag;
    [SerializeField] private AudioClip audioClip;
    private IEnumerator coroutine;
    private GameObject shooter = null;
    private Rigidbody2D rigidBody;
    private int fireDirection = 1;
    private Vector2 bulletVelocity;

    private void Awake()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        coroutine = Delete(lifeTime);
        bulletVelocity = new Vector2 (transform.position.x, transform.position.y);
        StartCoroutine(coroutine);
        if (audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (speed>0) BulletMovement();
    }

    private void BulletMovement()
    {
        rigidBody.velocity = bulletVelocity;
    }

    public void SetShooter(GameObject assignedShooter)
    {
        shooter = assignedShooter;
        //Debug.Log("haha!");
    }
    
    public void BeHorizontalBullet(bool isRight)
    {
        if (!isRight) fireDirection = -1;
        bulletVelocity = new Vector2 (fireDirection * speed, transform.position.y);
        //Debug.Log("Fire!");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent != null)
        {
            if (other.CompareTag(hitTag) || other.transform.parent.CompareTag(hitTag))
            {
                if (!CheckIfItIsShooter(other.gameObject))
                {
                    Health health = other.GetComponent<Health>();
                    if (health == null)
                    {
                        health = other.GetComponentInParent<Health>();
                    }
                    if (health != null)
                    {
                        health.SetDamage(minDamage, maxDamage);
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    private bool CheckIfItIsShooter(GameObject gameObject)
    {
        bool isTrue = false;
        if (gameObject.transform.parent != null)
        {
            isTrue = (gameObject.transform.parent.gameObject == shooter);
        }
        else
        {
            isTrue = (gameObject == shooter);
        }
        return isTrue;
    }

    private IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
