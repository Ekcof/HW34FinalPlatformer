using Nameofthegame.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject deathscreen;
    [SerializeField] private int goldPoints;
    [SerializeField] private Text goldText;
    private GameObject uICanvas;
    private LevelManager levelManager;
    private PlayerMovement playerInput;

    private void Awake()
    {
        Cursor.visible = true;
        uICanvas = GameObject.Find("UICanvas");
        levelManager = uICanvas.GetComponent<LevelManager>();
    }

    public void SetOnDeath()
    {
        deathscreen.SetActive(true);
        Cursor.visible = true;
        playerInput = GetComponent<PlayerMovement>();
        Destroy(playerInput);
    }

    public void AddGold(int goldPrice)
    {
        goldPoints += goldPrice;
        goldText.text = "x" + goldPoints;
    }

    public int GetGold()
    {
        return goldPoints;
    }
}
