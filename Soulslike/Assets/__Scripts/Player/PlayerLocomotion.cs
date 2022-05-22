using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Transform cameraObject;
    InputHandler inputHandler;
    PlayerManager playerManager;
    public Rigidbody rigidbody;
        
    public Vector3 moveDirection;

    [HideInInspector] public Transform myTransform;
    [HideInInspector] public PlayerAnimatorManager animationHandler;

    public GameObject normalCamera;

    [Header("Movement Stats")]
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float fallingSpeed = 45f;

    public bool isSprinting;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animationHandler = GetComponentInChildren<PlayerAnimatorManager>();
    }

    private void Start()
    {
        cameraObject = Camera.main.transform;
        myTransform = transform;

        animationHandler.Initialize();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isSprinting = inputHandler.b_Input;
        inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting();
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
            return;

        //if (playerManager.isInteracting)
        //    return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
            isSprinting = true;
            moveDirection *= speed;
            //playerStats.TakeStaminaDamage(sprintStaminaCost);
        }
        else
        {
            if (inputHandler.moveAmount < 0.5f)
            {
                moveDirection *= walkingSpeed;
                isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animationHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, isSprinting);

        if (animationHandler.canRotate)
        {
            HandlerRotation(delta);
        }
    }

    public void HandlerRotation(float delta)
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
    public void HandleRollingAndSprinting()
    {
        if (animationHandler.anim.GetBool("isInteracting"))
            return;

        //if (playerStats.currentStamina <= 0)
        //    return;

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animationHandler.PlayTargetAnimation("Roll", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
                //playerStats.TakeStaminaDamage(rollStaminaCost);
            }
            else
            {
                animationHandler.PlayTargetAnimation("BackStep", true);
                //playerStats.TakeStaminaDamage(backStabStaminaCost);
            }
        }
    }

    #endregion
}
