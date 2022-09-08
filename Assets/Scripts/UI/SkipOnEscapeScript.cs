using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameofthegame.Inputs
{
    public class SkipOnEscapeScript : MonoBehaviour
    {
        [SerializeField] private LevelManager levelmanager; 

        // Update is called once per frame
        private void Update()
        {
            bool isPause = Input.GetButtonDown(GameNamespace.CANCEL);
            if (isPause) { EscapeIsPressed(); }
        }
        
        private void EscapeIsPressed()
        {
            levelmanager.NextLevel();
        }

    }
}
