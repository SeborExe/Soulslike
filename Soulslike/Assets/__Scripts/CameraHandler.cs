using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    InputHandler inputHandler;

    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTranfroms;
    Vector3 cameraTransformPosition;
    public LayerMask ignoreLayers;
    public LayerMask enviromentLayer;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler sigleton;

    [Header("Camera settings")]
    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
    public float minimumPivot = -35f;
    public float maximumPivot = 35f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public float lockedPivotPosition = 2.25f;
    public float unlockPivotPosition = 1.65f;

    private void Awake()
    {
        sigleton = this;
        myTranfroms = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        inputHandler = FindObjectOfType<InputHandler>();
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (myTranfroms.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);

        myTranfroms.position = targetPosition;
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYinput)
    {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYinput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTranfroms.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
    }
}
