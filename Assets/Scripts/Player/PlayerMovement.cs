using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;


namespace Nameofthegame.Inputs
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float speed;

        [Header("Additional Settings")]
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private Transform groundColTransform;
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
        [SerializeField] private GameObject markerObject3;
        [SerializeField] private GameObject markerObject4;

        [SerializeField] private float dispersion = 0.05f;



        public bool isRight = true;

        private Animator animator;
        private float slopeCheckDistance;
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
        private int collisionNumber;
        private RaycastHit2D hit;
        private GameObject uICanvas;
        private int appDirection;
        private bool appJump;
        private bool appFire;
        private bool hasGround;
        private bool isHanging;
        private float yScaleHalf;
        private float yScaleQuarter;

        public bool IsHanging => isHanging;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            Cursor.visible = false;
            uICanvas = GameObject.Find("UICanvas");
            if (uICanvas != null) levelManager = uICanvas.GetComponent<LevelManager>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            makeHit = GetComponent<MakeHitScript>();
            groundColTransform = transform.GetChild(0);
            if (groundColTransform != null) groundCollider = groundColTransform.GetComponent<Collider2D>();
            yScale = groundCollider.bounds.size.y;
            xScale = groundCollider.bounds.size.x;
            slopeCheckDistance = xScale / 2 + 0.1f;
            yScaleHalf = yScale / 2;
            yScaleQuarter = yScale / 4;
        }

        private void Update()
        {
            if (isHanging) return;
            float horizontalDirection = Input.GetAxis(GameNamespace.HORIZONTAL_AXIS);
            if (appDirection != 0) horizontalDirection = appDirection;
            bool isJumping;
            if (appJump == false)
            {
                isJumping = Input.GetButtonDown(GameNamespace.JUMP);
            }
            else
            {
                isJumping = true;
            }

            if (isHanging) return;
            bool isUsing = Input.GetButtonDown(GameNamespace.SUBMIT2);
            bool isPause = Input.GetButtonDown(GameNamespace.CANCEL);

            CheckGround();
            SlopeCheck();

            Move(horizontalDirection, isJumping, isUsing, isPause);

            if (isGrounded)
            {
                //isJumping = false;
                canJump = true;
                if (rb.velocity.y <= 0) animator.SetBool("Jump", false);
                animator.SetBool("Fall", false);
            }
            else
            {
                animator.SetBool("Fall", true);
            }
        }

        public void SetDirection(int newValue)
        {
            appDirection = newValue;
        }

        public void JumpButtonState(bool isJumpingByButton)
        {
            appJump = isJumpingByButton;
        }

        /// <summary>
        /// Check the slope of the platform
        /// </summary>
        private void SlopeCheck()
        {
            Vector2 checkPos = new Vector2(groundColTransform.position.x, groundColTransform.position.y + jumpOffset);
            Vector2 upVector = new Vector2(checkPos.x, checkPos.y + 1f);
            SlopeCheckHorizontal(checkPos, upVector);
            SlopeCheckVertical(checkPos, upVector);
        }

        private void SlopeCheckHorizontal(Vector2 checkPos, Vector2 upVector)
        {
            Vector2 rightPoint = new Vector2(groundColTransform.position.x + xScale / 2, groundColTransform.position.y + jumpOffset);
            Vector2 leftPoint = new Vector2(groundColTransform.position.x - xScale / 2, groundColTransform.position.y + jumpOffset);
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

        /// <summary>
        /// Check if there is any collision with ground
        /// </summary>
        /// <param name="overlapPosition">touch point</param>
        /// <returns></returns>
        private bool GetGroundCollision(Vector2 overlapPosition)
        {
            bool hasGround = false;
            RaycastHit2D[] allCollisions = Physics2D.CircleCastAll(overlapPosition, xScale / 2, new Vector2(0, 0), 0, layerMask);
            if (allCollisions.Length > 0)
            {
                for (int i = 0; i < allCollisions.Length; i++)
                {
                    Vector2 hitPoint = allCollisions[i].point;
                    if (markerObject != null)
                    {
                        markerObject.transform.position = hitPoint;
                    }
                    if (hitPoint.y <= (overlapPosition.y + dispersion))
                    {
                        hasGround = true;
                        return hasGround;
                        Debug.Log(allCollisions.Length);
                    }
                }
            }
            Debug.Log(allCollisions.Length);
            return hasGround;
        }

        /// <summary>
        /// Check if there is ground below
        /// </summary>
        private void CheckGround()
        {
            Vector2 overlapPosition = new Vector2(groundColTransform.position.x, groundColTransform.position.y + dispersion);

            hasGround = GetGroundCollision(overlapPosition);
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

        public void Move(float appliedDirection, bool isJumping, bool isUsing, bool isPause)
        {
            direction = appliedDirection;
            if (isPause)
            {
                if (levelManager != null) levelManager.PauseGame();
            }
            if (isUnderControl)
            {
                if (isJumping) Jump();

                if (isHanging) return;

                if (Mathf.Abs(direction) > 0.1f)
                {
                    CheckRight(direction);
                    HorizontalMovement(direction);
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
                animator.SetBool("Fall", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else
            {
                (Vector2 newPoint, Transform edgeTransform) = GetTheEdge();
                if (newPoint != Vector2.zero)
                {
                    markerObject3.transform.position = newPoint;
                    HangOnTheEdge(newPoint, edgeTransform);
                }
            }
        }

        private void HangOnTheEdge(Vector2 pointToHang, Transform edgeTransform)
        {
            Vector2 upperPoint = pointToHang - new Vector2(groundCollider.bounds.extents.x * 2.5f, groundCollider.bounds.extents.y * 2.1f* (isRight ? 1 : -1));
            transform.position = upperPoint;
            transform.SetParent(edgeTransform, false);
            rb.isKinematic = true;
            rb.velocity = new Vector2(0, 0);
            isUnderControl = false;
            isHanging = true;
            animator.SetBool("Fall", false);
            animator.SetBool("Run", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Edge", true);
        }

        private (Vector2, Transform) GetTheEdge()
        {
            Vector2 topPoint = groundCollider.bounds.max;

            int rightDirection = isRight ? 1 : -1;

            Vector2 upperClimpPoint = topPoint + (Vector2.up + Vector2.right * rightDirection) * yScaleQuarter;
            markerObject4.transform.position = upperClimpPoint;

            RaycastHit2D upperHit = Physics2D.Raycast(topPoint, (Vector2.up + Vector2.right * rightDirection) * yScaleQuarter, yScaleHalf, layerMask);

            if (upperHit.collider == null)
            {
                RaycastHit2D downHit = Physics2D.Raycast(upperClimpPoint, Vector2.down, yScaleHalf, layerMask);
                Vector2 prevPoint;
                Transform prevTransform;
                if (downHit.collider != null)
                {
                    prevPoint = downHit.collider.ClosestPoint(downHit.point);
                    prevTransform = downHit.transform;
                }
                else
                {
                    return (Vector2.zero, null);
                }

                while (downHit.collider != null)
                {
                    upperClimpPoint.x -= 0.01f * rightDirection;
                    downHit = Physics2D.Raycast(upperClimpPoint, Vector2.down, yScaleHalf, layerMask);
                    if (downHit.collider == null)
                    {
                        return (prevPoint, prevTransform);
                    }
                    prevPoint = downHit.collider.ClosestPoint(downHit.point);
                    prevTransform = downHit.transform;
                    //                if (downHit.collider != null) return (downHit.collider.ClosestPoint(downHit.point), downHit.transform);
                }
            }
            return (Vector2.zero, null);
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
            else if (!isGrounded && !GetAnyCollision()) //If in the air
            {
                if (rb.velocity.y >= 0)
                {
                    Debug.Log("AIR!!!!");
                    newVelocity.Set(speed * direction, rb.velocity.y);
                    rb.velocity = newVelocity;
                }
            }
        }

        /// <summary>
        /// Get true if player is turned right
        /// </summary>
        /// <param name="direction"></param>
        public void CheckRight(float direction)
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

        private bool GetAnyCollision()
        {
            if (collisionNumber > 0) return true;
            return false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ++collisionNumber;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            --collisionNumber;
        }
    }

}