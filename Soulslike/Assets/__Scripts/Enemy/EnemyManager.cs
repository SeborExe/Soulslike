using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStatsManager enemyStatsManager;
    EnemyEffectManager enemyEffectManager;

    public State currentState;
    public CharacterStatsManager currentTarget;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;

    public bool isPerformingAction;

    public float rotationSpeed = 15f;
    public float maximumAggroRadius = 1.5f;
    public float moveSpeed = 1f;

    [Header("A.I. Settings")]
    public float detectionRadius = 20f;
    public float minimumDetectionAngle = -50f;
    public float maximumDetectionAngle = 50f;

    [Header("AI combat setting")]
    public bool allAIToPerformCombos = true;
    public float comboLikelyHood = 50;

    [Header("Combat flags")]
    public bool isPhaseShifting;

    public float currentRecoveryTime = 0;

    protected override void Awake()
    {
        base.Awake();
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyEffectManager = GetComponent<EnemyEffectManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isUsingLeftHand = enemyAnimatorManager.animator.GetBool("isUsingLeftHand");
        isUsingRightHand = enemyAnimatorManager.animator.GetBool("isUsingRightHand");
        isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
        isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
        isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
        canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
        isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
        enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);

        HandlePlayerDead();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        enemyEffectManager.HandleAllBuildUpEffects();
    }

    private void LateUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    void SwitchToNextState(State state)
    {
        currentState = state;
    }

    void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    void HandlePlayerDead()
    {
        if (currentTarget != null && currentTarget.isDead)
        {
            currentTarget = null;
            currentState = null;
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0, Time.deltaTime);
            enemyAnimatorManager.animator.SetFloat("Horizontal", 0, 0, Time.deltaTime);
        }
    }
}
