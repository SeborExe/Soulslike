using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    Transform cameraObject;
    InputHandler inputHandler;
    PlayerManager playerManager;
    CameraHandler cameraHandler;
    PlayerStatsManager playerStatsManager;
    public Rigidbody rigidbody;
        
    public Vector3 moveDirection;

    [HideInInspector] public Transform myTransform;
    [HideInInspector] public PlayerAnimatorManager playerAnimationManager;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlocker;

    public GameObject normalCamera;

    [Header("Movement Stats")]
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float rotationSpeed = 16f;
    [SerializeField] float fallingSpeed = 475f;

    [Header("Ground & Air states")]
    [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] float groundDirectionRayDistance = 0.2f;
    float groundDetectionRayStartPoint = 0.5f;
    public LayerMask groundLayer;
    public float inAirTimer;

    [Header("Stamina costs")]
    [SerializeField] int rollStaminaCost = 15;
    [SerializeField] int backStabStaminaCost = 12;
    [SerializeField] int sprintStaminaCost = 1;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        playerAnimationManager = GetComponent<PlayerAnimatorManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    private void Start()
    {
        cameraObject = Camera.main.transform;
        myTransform = transform;

        playerManager.isGrounded = true;
        groundLayer = ~(1 << 8 | 1 << 11);
        Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }

    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleMovement(float delta)
    {
        if (playerStatsManager.isDead) return;

        if (inputHandler.rollFlag) return;

        if (playerManager.isInteracting) return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
            playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
        }
        else
        {
            if (inputHandler.moveAmount < 0.5f)
            {
                moveDirection *= walkingSpeed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
        {
            playerAnimationManager.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
        }
        else
        {
            playerAnimationManager.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
        }

        if (playerAnimationManager.canRotate)
        {
            HandlerRotation(delta);
        }
    }
    public void HandlerRotation(float delta)
    {
        if (playerManager.animator.GetBool("isDead") == true) return;

        if (playerAnimationManager.canRotate)
        {
            if (inputHandler.lockOnFlag)
            {
                if (inputHandler.sprintFlag || inputHandler.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                Vector3 targetDirection = Vector3.zero;
                float moveOveride = inputHandler.moveAmount;

                targetDirection = cameraObject.forward * inputHandler.vertical;
                targetDirection += cameraObject.right * inputHandler.horizontal;

                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                    targetDirection = myTransform.forward;

                float rs = rotationSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDirection);
                Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                myTransform.rotation = targetRotation;
            }
        }
    }
    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimationManager.animator.GetBool("isInteracting"))
            return;

        if (playerStatsManager.currentStamina <= 0)
            return;

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                playerAnimationManager.PlayTargetAnimation("Roll", true);
                playerAnimationManager.EraseHandIKForWeapon();
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
                playerStatsManager.TakeStaminaDamage(rollStaminaCost);
            }
            else
            {
                playerAnimationManager.PlayTargetAnimation("BackStep", true);
                playerAnimationManager.EraseHandIKForWeapon();
                playerStatsManager.TakeStaminaDamage(backStabStaminaCost);
            }
        }
    }
    public void HandleFall(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 8f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundLayer))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in air for: " + inAirTimer);
                    playerAnimationManager.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    playerAnimationManager.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }

        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                    playerAnimationManager.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            myTransform.position = targetPosition;
        }

        if (playerManager.isGrounded)
        {
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }
    }
    public void HandleJumping()
    {
        if (playerManager.isInteracting) return;

        if (playerStatsManager.currentStamina <= 0)
            return;

        if (inputHandler.jump_Input)
        {
            if (inputHandler.moveAmount > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                playerAnimationManager.PlayTargetAnimation("Jump", true);
                playerAnimationManager.EraseHandIKForWeapon();
                moveDirection.y = 0;
                Quaternion JumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = JumpRotation;
            }
        }
    }
}
