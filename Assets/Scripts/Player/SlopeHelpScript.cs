using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeHelpScript : MonoBehaviour
{
    [SerializeField] private float newJumpOffset;
    private Transform hero;
    private Nameofthegame.Inputs.PlayerMovement playerMovement;
    private float oldJumpOffset;
    private void Awake()
    {

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        GetPlayerAndChangeJumpOffset(other.gameObject, newJumpOffset);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GetPlayerAndChangeJumpOffset(other.gameObject, oldJumpOffset);
    }

    private void GetPlayerAndChangeJumpOffset(GameObject gameObject, float jumpOffset)
    {
        if (gameObject.layer == LayerMask.NameToLayer("player"))
        {
            playerMovement = gameObject.GetComponent<Nameofthegame.Inputs.PlayerMovement>();
            if (playerMovement == null)
            {
                playerMovement = gameObject.GetComponentInParent<Nameofthegame.Inputs.PlayerMovement>();
            }
            else { return; }
            oldJumpOffset = playerMovement.ReturnJumpOffset();
            newJumpOffset = jumpOffset;
            playerMovement.ChangeJumpOffset(jumpOffset);
        }

    }
}
