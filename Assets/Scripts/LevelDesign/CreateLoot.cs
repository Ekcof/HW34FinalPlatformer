using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLoot : MonoBehaviour
{

    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private float delay;
    [SerializeField] private float attitude;
    private IEnumerator coroutine;

    public void CreatePrefab()
    {
        //Debug.Log("Creating Prefab!");
        coroutine = Delayed(delay);
        StartCoroutine(coroutine);
    }

    private IEnumerator Delayed (float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector2 prefabPosition = new Vector2(transform.position.x, transform.position.y + attitude);
        Instantiate(lootPrefab, prefabPosition, Quaternion.identity);
    }

}
