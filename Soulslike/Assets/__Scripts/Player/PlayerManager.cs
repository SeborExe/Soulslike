using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : CharacterManager
{
    public Animator anim;
    InputHandler inputHandler;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    PlayerAnimatorManager playerAnimatorManager;
    InteractableUI interactableUI;
    PlayerStats playerStats;

    [Header("Player flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isInvulnerable;

    [Header("Interactable objects")]
    [SerializeField] float fadeSpeed = 0.2f;
    [SerializeField] GameObject interactableUIGameObject;
    public GameObject itemInteractableObject;
    bool hide = false;
    float originalTransparency;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        interactableUI = FindObjectOfType<InteractableUI>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isUsingRightHand = anim.GetBool("isUsingRightHand");
        isUsingLeftHand = anim.GetBool("isUsingLeftHand");
        isInvulnerable = anim.GetBool("isInvulnerable");
        isFiringSpell = anim.GetBool("isFiringSpell");
        anim.SetBool("isInAir", isInAir);
        anim.SetBool("isDead", playerStats.isDead);
        anim.SetBool("isBlocking", isBlocking);
        playerAnimatorManager.canRotate = anim.GetBool("canRotate");

        inputHandler.TickInput(delta);
        playerLocomotion.HandleJumping();
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerStats.RegenerateStamina();

        CheckForInteractable();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandlerRotation(delta);
        playerLocomotion.HandleFall(delta, playerLocomotion.moveDirection);

        if (hide)
        {
            SlowlyHideText();
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
        inputHandler.d_pad_up = false;
        inputHandler.d_pad_down = false;
        inputHandler.d_pad_left = false;
        inputHandler.d_pad_right = false;
        inputHandler.a_Input = false;
        inputHandler.jump_Input = false;
        inputHandler.inventory_Input = false;
        inputHandler.critical_attack_Input = false;
        inputHandler.lt_Input = false;

        float delta = Time.deltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }

    #region Player Interactions
    public void CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, -0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (inputHandler.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }

        else
        {
            if (interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }
        }
    }
    
    public void OpenChectInteraction(Transform playrStandingPosition)
    {
        playerLocomotion.rigidbody.velocity = Vector3.zero; //Stop player
        transform.position = playrStandingPosition.transform.position;
        playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
    }

    public void PassThroughtFogWallInteraction(Transform fogWallEntrance)
    {
        playerLocomotion.rigidbody.velocity = Vector3.zero; //Stop player
        Vector3 rotationDirection = fogWallEntrance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;
        //Rotate over time

        playerAnimatorManager.PlayTargetAnimation("Pass_Throught_Fog", true);
    }

    #endregion

    #region Show text after pick up item
    public void PickUpItem()
    {
        itemInteractableObject.SetActive(true);
        StartCoroutine(HideTextObjectCoroutine());
    }

    IEnumerator HideTextObjectCoroutine()
    {
        yield return new WaitForSeconds(2f);
        hide = true;
    }

    private void SlowlyHideText()
    {
        Color color = itemInteractableObject.GetComponent<Image>().color;

        color.a -= Time.deltaTime * fadeSpeed;
        Color newColor = new Color(0, 0, 0, color.a);
        itemInteractableObject.GetComponent<Image>().color = newColor;

        if (itemInteractableObject != null && (itemInteractableObject.GetComponent<Image>().color.a <= 0.01 || inputHandler.a_Input))
        {
            itemInteractableObject.GetComponent<Image>().color = new Color(0, 0, 0, originalTransparency);
            itemInteractableObject.SetActive(false);
            hide = false;
        }
    }
    #endregion
}
