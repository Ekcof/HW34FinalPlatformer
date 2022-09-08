using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public int level;
    private GameObject gameBrain;

    // Start is called before the first frame update
    private void Awake()
    {
        gameBrain = GameObject.Find("GameBrain");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
