using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nameofthegame.Inputs
{
    public class WinTriggerScript : MonoBehaviour
    {
        [SerializeField] private GameObject UISystem;
        [SerializeField] private bool isLastLevel;
        private LevelManager levelManager;
        private IEnumerator coroutine;
        private GameObject hero;

        private void Awake()
        {
            Cursor.visible = true;
            hero = GameObject.Find("Hero");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.parent.gameObject.layer == 8)
            {
                Cursor.visible = true;
                Destroy(hero.GetComponent<PlayerMovement>());
                levelManager = UISystem.GetComponent<LevelManager>();
                PlayerBehavior playerBehavior = hero.GetComponent<PlayerBehavior>();
                int gold = playerBehavior.GetGold();
                levelManager.WinMenu(gold);
                if (!isLastLevel)
                {
                    coroutine = NextLevelDelay(1.5f, playerBehavior);
                    StartCoroutine(coroutine);
                }
                else
                {
                    Debug.Log("Last level!");
                    GameObject winPanel = UISystem.transform.Find("WinPanel").gameObject;
                    if (winPanel != null)
                    {
                        GameObject nextLevelButton = winPanel.transform.Find("NextLevelButton").gameObject;
                        if (nextLevelButton != null) nextLevelButton.SetActive(false);
                    }
                    levelManager.EndPanel();
                }
                levelManager.CalculateLevelParameters();
            }
        }

        IEnumerator NextLevelDelay(float waitFor, PlayerBehavior playerBehavior)
        {
            yield return new WaitForSeconds(waitFor);
            levelManager.NextLevel();
            //levelManager.NextLevel();
        }

        public void RatingSum(int buildIndex, int rating)
        {
            GlobalControl.Instance.levelRating[buildIndex] = rating;
        }

    }
}