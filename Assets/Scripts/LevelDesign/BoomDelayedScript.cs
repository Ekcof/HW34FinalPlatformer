using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomDelayedScript : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private float delay;
    private IEnumerator coroutine;

    void Start()
    {
        coroutine = Explode(delay);
        StartCoroutine(coroutine);
    }

    private IEnumerator Explode (float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
