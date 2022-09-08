using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace Nameofthegame.Inputs
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float speed;

        [Header("Additional Settings")]
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private Transform groundControlColliderTransform;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float jumpOffset;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float submitDistance = 0.32f;
        [SerializeField] LevelManager levelManager;
        [SerializeField] private PhysicsMaterial2D fullFriction;
        [SerializeField] private PhysicsMaterial2D noFriction;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private GameObject markerObject;
        [SerializeField] private GameObject markerObject2;

        private Animator animator;
        private float slopeCheckDistance;
        private bool isRight = true;
        private bool isOnSlope = false;
        private bool isJumping;
        private bool canJump = true;
        private bool canWalkOnSlope;
        private float slopeSideAngle;
        private float slopeDownAngle;
        private float lastSlopeAngle;
        private float direction;
        private Vector2 slopeNormalPerp;
        private Vector2 newVelocity;
        private MakeHitScript makeHit;
        private bool isUnderControl = true;
        private Rigidbody2D rb;
        private new Renderer renderer;
        private float yScale;
        private float xScale;
        private Collider2D groundCollider;
        private RaycastHit2D hit;
        private GameObject uICanvas;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            yScale = renderer.bounds.size.y;
            xScale = renderer.bounds.size.x;
            Cursor.visible = false;
            uICanvas = GameObject.Find("UICanvas");
            if (uICanvas != null) levelManager = uICanvas.GetComponent<LevelManager>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            makeHit = GetComponent<MakeHitScript>();
            groundCollider = transform.GetChild(0).GetComponent<Collider2D>();
            slopeCheckDistance = xScale / 2 + 0.1f;
        }

        private void Update()
        {
            float horizontalDirection = Input.GetAxis(GameNamespace.HORIZONTAL_AXIS);
            bool isJumping = Input.GetButtonDown(GameNamespace.JUMP);
            bool isFiring1 = Input.GetButtonDown(GameNamespace.FIRE1);
            bool isUsing = Input.GetButtonDown(GameNamespace.SUBMIT2);
            bool isPause = Input.GetButtonDown(GameNamespace.CANCEL);
            //CheckOverlap();
            CheckGround();
            SlopeCheck();
            Move(horizontalDirection, isJumping, isFiring1, isUsing, isPause);
            if (isGrounded)
            {
                isJumping = false;
                canJump = true;
                animator.SetBool("Jump", false);
            }

        }

        /// <summary>
        /// Check the slope of the platform
        /// </summary>
        private void SlopeCheck()
        {
            Vector2 checkPos = new Vector2(transform.position.x, transform.position.y + jumpOffset);
            Vector2 upVector = new Vector2(checkPos.x, checkPos.y + 1f);
            SlopeCheckHorizontal(checkPos, upVector);
            SlopeCheckVertical(checkPos, upVector);
        }

        private void SlopeCheckHorizontal(Vector2 checkPos, Vector2 upVector)
        {
            Vector2 rightPoint = new Vector2(transform.position.x + xScale / 2, transform.position.y + jumpOffset);
            Vector2 leftPoint = new Vector2(transform.position.x - xScale / 2, transform.position.y + jumpOffset);
            RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, Vector2.right, slopeCheckDistance, layerMask);
            RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, Vector2.left, slopeCheckDistance, layerMask);

            Debug.DrawLine(checkPos, rightPoint, Color.red);

            if (markerObject2 != null)
            {
                markerObject2.transform.position = checkPos;
            }

            if (slopeHitFront)
            {
                isOnSlope = true;
                slopeSideAngle = Vector2.Angle(slopeHitFront.point, upVector);

            }
            else if (slopeHitBack)
            {
                isOnSlope = true;
                slopeSideAngle = Vector2.Angle(slopeHitBack.point, upVector);
            }
            else
            {
                slopeSideAngle = 0.0f;
                isOnSlope = false;
            }

        }

        private void SlopeCheckVertical(Vector2 checkPos, Vector2 upVector)
        {
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, layerMask);

            if (hit)
            {

                slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

                slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeDownAngle != lastSlopeAngle)
                {
                    isOnSlope = true;
                }

                lastSlopeAngle = slopeDownAngle;

                Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
                Debug.DrawRay(hit.point, hit.normal, Color.green);

            }

            if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
            {
                canWalkOnSlope = false;
            }
            else
            {
                canWalkOnSlope = true;
            }
        }

        private bool GetCollision(Vector2 overlapPosition)
        {
            bool hasGround = false;
            RaycastHit2D[] allCollisions = Physics2D.CircleCastAll(overlapPosition, xScale, new Vector2(0, 0), 0, layerMask);
            if (allCollisions.Length > 0)
            {
                for (int i=0; i < allCollisions.Length; i++)
                {
                    Vector2 hitPoint = allCollisions[i].point;
                    if (markerObject != null)
                    {
                        markerObject.transform.position = hitPoint;
                    }
                    if (hitPoint.y <= overlapPosition.y)
                    {
                        hasGround = true;
                    }
                }
            }
            return hasGround;
        }

        private void CheckGround()
        {
            Vector2 overlapPosition = new Vector2(transform.position.x, transform.position.y + 0.05f);

            bool hasGround = GetCollision(overlapPosition);
            isGrounded = Physics2D.OverlapCircle(overlapPosition, jumpOffset, layerMask) && hasGround;
            if (rb.velocity.y <= 0.0f)
            {
                isJumping = false;
            }
            if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
            {
                canJump = true;
            }

            if (isOnSlope && canWalkOnSlope && direction == 0.0f && isGrounded)
            {
                groundCollider.sharedMaterial = fullFriction;
                rb.sharedMaterial = fullFriction;
            }
            else
            {
                groundCollider.sharedMaterial = noFriction;
                rb.sharedMaterial = noFriction;
            }
        }

        public void LoseControl()
        {
            isUnderControl = false;
        }

        public void ReturnControl()
        {
            isUnderControl = true;
        }

        public void Move(float appliedDirection, bool isJumping, bool isFiring1, bool isUsing, bool isPause)
        {
            direction = appliedDirection;
            if (isPause)
            {
                if (levelManager !=null) levelManager.PauseGame();
            }
            if (isUnderControl)
            {
                if (isJumping) Jump();
                if (isFiring1)
                {
                    animator.SetBool("Attack", true);
                    makeHit.CreateAttackCollider(isRight);
                }
                else
                {
                    if (Mathf.Abs(direction) > 0.1f)
                    {
                        CheckRight(direction);
                        HorizontalMovement(direction);
                    }
                }
                if (direction == 0)
                {
                    animator.SetBool("Run", false);
                }
                if (isUsing)
                {
                    FindButton();
                }
            }
        }

        private void FindButton()
        {
            GameObject[] gameObjects;
            gameObjects = GameObject.FindGameObjectsWithTag("Button");

            if (gameObjects.Length != 0)
            {
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector2 position = transform.position;
                foreach (GameObject gameObject in gameObjects)
                {
                    Vector2 diff = (Vector2)gameObject.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = gameObject;
                        distance = curDistance;
                    }
                }
                if (distance < submitDistance)
                {
                    Animator animatorButton = closest.GetComponent<Animator>();
                    if (animatorButton != null)
                    {
                        if (animatorButton.GetBool("Pressed"))
                        {
                            animatorButton.SetBool("Pressed", false);
                        }
                        else
                        {
                            animatorButton.SetBool("Pressed", true);
                        }
                    }
                }
                else
                {
                    Debug.Log("Button is far from player!" + distance);
                }
            }
        }

        private void Jump()
        {
            if (isGrounded && canJump)
            {
                isGrounded = false;
                animator.SetBool("Jump", true);
                isJumping = true;
                canJump = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        private void HorizontalMovement(float direction)
        {
            animator.SetBool("Run", true);

            if (isGrounded && !isOnSlope && !isJumping && Mathf.Abs(rb.velocity.y) < 0.1f) //if not on slope
            {
                newVelocity.Set(speed * direction, 0.0f);
                rb.velocity = newVelocity;
            }
            else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
            {
                newVelocity.Set(speed * slopeNormalPerp.x * -direction, speed * slopeNormalPerp.y * -direction);
                rb.velocity = newVelocity;
            }
            else if (!isGrounded) //If in the air
            {
                newVelocity.Set(speed * direction, rb.velocity.y);
                rb.velocity = newVelocity;
            }
        }

        private void CheckRight(float direction)
        {
            Vector3 theScale = transform.localScale;
            if (direction < 0 && isRight)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
                isRight = false;
            }
            if (direction > 0 && !isRight)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
                isRight = true;
            }
        }

        public void ChangeJumpOffset(float newValue)
        {
            jumpOffset = newValue;
        }

        public float ReturnJumpOffset()
        {
            return jumpOffset;
        }
    }

}