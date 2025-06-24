using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterFacing : MonoBehaviour
{
    [SerializeField] Transform rotate;
    [SerializeField] private bool isCameraFollow = false;
    [SerializeField] private CameraFollowObject cameraFollowObject;
    private IMove move;
    public bool IsFacingRight { get; private set; } = true;
    public bool isFasingRight;
    private float fallSpeedYDampingChangeThreshold;

    public void Awake()
    {
        move = rotate.parent.GetComponent<IMove>();
        isFasingRight = IsFacingRight;
        fallSpeedYDampingChangeThreshold = CameraManager.Instance._fallSpeedYDampingChangeThreshold;
    }
    public void UpdateDirection()
    {
        if (move.XVelocity > 0.01f && !IsFacingRight)
        {
            IsFacingRight = true;
            isFasingRight = IsFacingRight;
            rotate.eulerAngles = Vector3.zero;
            cameraFollowObject.CallTurn();
        }
        else if (move.XVelocity < -0.01f && IsFacingRight)
        {
            IsFacingRight = false;
            IsFacingRight = IsFacingRight;
            rotate.eulerAngles = new Vector3(0, 180, 0);
            cameraFollowObject.CallTurn();
        }
    }
    private void FixedUpdate()
    {
        if (isCameraFollow)
        {
            UpdateYCameraDamping();
            UpdateDirection();
        }

    }

    private void UpdateYCameraDamping()
    {
        if (move.YVelocity < fallSpeedYDampingChangeThreshold &&
            !CameraManager.Instance.IsLerpingYDamping &&
            !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            Debug.Log("[CharacterFacing] Detected falling. Triggering camera lerp to fall state.");
            CameraManager.Instance.LerpYDamping(true);
        }

        if (move.YVelocity >= 0 &&
            !CameraManager.Instance.IsLerpingYDamping &&
            CameraManager.Instance.LerpedFromPlayerFalling)
        {
            Debug.Log("[CharacterFacing] Falling ended. Triggering camera return to normal.");
            CameraManager.Instance.LerpedFromPlayerFalling = false;
            CameraManager.Instance.LerpYDamping(false);
        }
    }
}
