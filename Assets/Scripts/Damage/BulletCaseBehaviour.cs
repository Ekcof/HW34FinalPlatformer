using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class BulletCaseBehaviour : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float speed;
    private IEnumerator coroutine;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        coroutine = caseLifetime(lifetime);
        rigidbody = GetComponent<Rigidbody2D>(); 
        StartCoroutine(coroutine);
        rigidbody.AddForce(Vector3.up * speed, ForceMode2D.Force);
    }

    private IEnumerator caseLifetime(float period)
    {
        yield return new WaitForSeconds(period);
        Destroy(gameObject);
    }
}
