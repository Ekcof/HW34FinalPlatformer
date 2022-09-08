using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Text[] levelLabels;
    [SerializeField] private UnityEngine.UI.Button[] levelButtons;
    [SerializeField] private GameObject continueButton;

    private void Awake()
    {
        UnityEngine.Cursor.visible = true;

        int levelNum = GlobalControl.Instance.levelRating.Length;
        for (int i = 0; i < levelNum; i++)
        {
            levelLabels[i].text = "x" + GlobalControl.Instance.levelRating[i];
        }
        int levelNum2 = levelButtons.Length;
        for (int i = 0; i < levelNum2; i++)
        {
            if (i >= GlobalControl.Instance.level)
            {
                levelButtons[i].gameObject.SetActive(false);
            }
            else
            {
                levelButtons[i].gameObject.SetActive(true);
            }
        }
        if (GlobalControl.Instance.level > 0) continueButton.SetActive(true);
    }

    public void ContinueGame ()
    {
        SceneManager.LoadScene(GlobalControl.Instance.level+2);
        Time.timeScale = 1f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
