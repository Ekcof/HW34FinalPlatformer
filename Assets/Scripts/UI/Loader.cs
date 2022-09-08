using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    private IEnumerator coroutine;
    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        coroutine = Wait(2.0f);
        StartCoroutine(coroutine);
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(1);
    }

}
