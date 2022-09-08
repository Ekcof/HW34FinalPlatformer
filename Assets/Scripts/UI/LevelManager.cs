
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private int index;
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private GameObject deadPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Text ratingText;
    [SerializeField] private Text enemiesText;
    [SerializeField] private Text goldText;
    [SerializeField] private int maxGold;
    [SerializeField] private Transform enemies;
    [SerializeField] private int maxEnemies;
    private int rating;
    private int buildIndex;
    private GameObject hero;
    public PlayerBehavior playerBehavior;
    private Nameofthegame.Inputs.PlayerMovement playerMovement;
    private IEnumerator coroutine;

    private void Awake()
    {
        if (maxEnemies > 0) maxEnemies = enemies.childCount;
        hero = GameObject.Find("Hero");
        if (hero != null)
        {
            playerBehavior = hero.GetComponent<PlayerBehavior>();
            playerMovement = hero.GetComponent<Nameofthegame.Inputs.PlayerMovement>();
        }
    }

    public int Rating()
    {
        return rating;
    }

    private void ActivateScene()
    {
        Time.timeScale = 1f;
        if (fadePanel != null)
        {
            fadePanel.SetActive(false);
        }
    }

    public int GetAliveEnemies()
    {
        int aliveEnemies = 0;
        int enemiesCount = enemies.childCount;
        for (int i = 0; i < enemiesCount; i++)
        {
            Transform enemy = enemies.GetChild(i);
            bool isAlive = enemy.GetComponent<Health>();
            if (isAlive)
            {
                ++aliveEnemies;
            }
        }
        return aliveEnemies;
    }

    public void OnLevelButtonClick()
    {
        SceneManager.LoadScene(index);

        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (Time.timeScale > 0f)
        {
            Cursor.visible = true;
            Time.timeScale = 0f;
            fadePanel.SetActive(true);
            playerMovement.LoseControl();
        }
        else
        {
            Cursor.visible = false;
            playerMovement.ReturnControl();
            ActivateScene();
        }
    }

    public void WinMenu(int gold)
    {
        int deadEnemies = maxEnemies - GetAliveEnemies();
        goldText.text = gold + "/" + maxGold;
        enemiesText.text = deadEnemies + "/" + maxEnemies;
        winPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void DeadMenu()
    {
        Cursor.visible = true;
        deadPanel.SetActive(true);
        Time.timeScale = 0.3f;
    }

    public void ControlsMenu()
    {
        GameObject controlMenu = transform.Find("ControlsPanel").gameObject;
        coroutine = ControlsCoroutine(controlMenu);
        if (controlMenu!=null && !controlMenu.activeSelf)
        {
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator ControlsCoroutine(GameObject controlMenu)
    {
        controlMenu.SetActive(true);
        yield return new WaitForSeconds(7f);
        controlMenu.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ActivateScene();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        if (GlobalControl.Instance.level == 0) AddLevelOnMenuPanel();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex + 1);
        ActivateScene();
    }

    public void CalculateLevelParameters()
    {
        if (playerBehavior != null)
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex - 3;
            int level = GlobalControl.Instance.level;
            int rating = GlobalControl.Instance.levelRating[buildIndex];
            int currentRating = playerBehavior.GetGold();
            if (currentRating > rating)
            {
                RatingSum(buildIndex, currentRating);
            }

            if (level <= (buildIndex + 2))
            {
                AddLevelOnMenuPanel();
            }
        }
    }

    private void AddLevelOnMenuPanel()
    {
        ++GlobalControl.Instance.level;
    }

    public void RatingSum(int buildIndex, int rating)
    {
        GlobalControl.Instance.levelRating[buildIndex] = rating;
    }

    public void EndPanel()
    {
        Cursor.visible = true;
        endPanel.SetActive(true);
        Time.timeScale = 0.3f;
    }
}
