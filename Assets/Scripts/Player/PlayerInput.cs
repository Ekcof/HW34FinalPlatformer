using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameofthegame.Inputs
{
    [RequireComponent(typeof(PlayerMovement))]

    public class PlayerInput : MonoBehaviour
    {
        private float horizontal;
        private PlayerMovement playerMovement;
        private Vector2 movement;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            float horizontalDirection = Input.GetAxis(GameNamespace.HORIZONTAL_AXIS);
            bool isJumping = Input.GetButtonDown(GameNamespace.JUMP);
            bool isFiring1 = Input.GetButtonDown(GameNamespace.FIRE1);
            bool isUsing = Input.GetButtonDown(GameNamespace.SUBMIT2);
            bool isPause = Input.GetButtonDown(GameNamespace.CANCEL);
            playerMovement.Move(horizontalDirection, isJumping, isUsing, isPause);
        }
    }
}