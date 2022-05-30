using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    //public State currentState;
    public CharacterStats currentTarget;
    //public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;
    EnemyLocomotionManager enemyLocomotionManager;

    public bool isPerformingAction;
    public bool isInteracting;

    public float rotationSpeed = 15f;
    public float maximumAttackRange = 1.5f;
    public float moveSpeed = 1f;

    [Header("A.I. Settings")]
    public float detectionRadius = 20f;
    public float minimumDetectionAngle = -50f;
    public float maximumDetectionAngle = 50f;

    public float currentRecoveryTime = 0;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
    }

    private void Update()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (currentTarget == null)
        {
            enemyLocomotionManager.HandleDetection();
        }
    }
}
