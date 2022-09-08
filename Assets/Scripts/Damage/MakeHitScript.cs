using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeHitScript : MonoBehaviour
{
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float hitDistance;
    [SerializeField] private float hitAltitude;
    [SerializeField] private float delay;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private bool isMelee;
    private BulletBehavior bulletBehavior;
    private Health health;
    private float speed;

    private IEnumerator coroutine;

    public void CreateAttackCollider(bool isRight)
    {
        coroutine = DelayedCreation(delay, isRight);
        StartCoroutine(coroutine);
    }


    private IEnumerator DelayedCreation(float delay, bool isRight)
    {
        yield return new WaitForSeconds(delay);
        float currentHitDistance = hitDistance;
        if (isMelee)
        {
            health = GetComponent<Health>();
            if (health == null || health.GetDamage() <= 0) { yield break; }
        }

        if (!isRight)
            hitDistance *= -1;
        Vector2 prefabPosition = new Vector2(transform.position.x + currentHitDistance * hitDistance, transform.position.y + hitAltitude);
        GameObject hit = Instantiate(hitPrefab, prefabPosition, Quaternion.identity);
        bulletBehavior = hit.GetComponent<BulletBehavior>();
        if (isMelee)
        {
            hit.transform.parent = transform;
        }
        else
        {
            if (hit.GetComponent<Rigidbody2D>())
            {
                bulletBehavior.BeHorizontalBullet(isRight);
            }
        }

        bulletBehavior.SetShooter(transform.gameObject);

    }
}
