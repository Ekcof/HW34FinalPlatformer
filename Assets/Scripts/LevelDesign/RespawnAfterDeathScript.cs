using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterDeathScript : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float delay;
    public GameObject prefab;
    [SerializeField] private bool invisibleOnDelay;
    private RespawnAfterDeathScript respawnAfterDeathScript;
    private IEnumerator coroutine;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    //Setting variables for a new GameObject
    public void SetVariables(Transform respawnPointChild, float delayChild, GameObject prefabChild, bool invisibleOnDelayChild)
    {
        if (gameObject == null) { return; }
        respawnPoint = respawnPointChild;
        delay = delayChild;
        prefab = prefabChild;
        invisibleOnDelay = invisibleOnDelayChild;
    }

    // Starting Respawn from a Health script
    public void StartRespawn()
    {
        if (invisibleOnDelay)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        coroutine = Respawn(delay);
        StartCoroutine(coroutine);
    }

    private IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject newOne = Instantiate(prefab, respawnPoint.position, Quaternion.identity);
        respawnAfterDeathScript = newOne.GetComponent<RespawnAfterDeathScript>();
        respawnAfterDeathScript.SetVariables(respawnPoint, delay, prefab, invisibleOnDelay);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
