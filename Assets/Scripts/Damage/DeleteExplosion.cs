using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteExplosion : MonoBehaviour
{
    private IEnumerator coroutine;

    private void Start()
    {
        coroutine = Delete(0.6f);
        StartCoroutine(coroutine);
    }

    private IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
