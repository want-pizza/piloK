using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterFacing : MonoBehaviour
{
    [SerializeField] Transform rotate;
    [SerializeField] private bool isCameraFollow = false;
    [SerializeField] private CameraFollowObject cameraFollowObject;
    [SerializeField] private AttackEventChannel eventChannel;
    private IMove move;
    public bool IsFacingRight { get; private set; } = true;
    private bool isFañingRight;
    private float fallSpeedYDampingChangeThreshold;
    private bool blockFacing = false;

    public void Awake()
    {
        move = rotate.parent.GetComponent<IMove>();
        isFañingRight = IsFacingRight;
        fallSpeedYDampingChangeThreshold = CameraManager.Instance._fallSpeedYDampingChangeThreshold;
    }
    private void OnEnable()
    {
        if (isCameraFollow)
        {
            eventChannel.OnAttackTriggered += TurnOnBlockFacing;
            eventChannel.OnAttackEnded += TurnOffBlockFacing;
        }
    }
    private void OnDisable()
    {
        if (isCameraFollow)
        {
            eventChannel.OnAttackTriggered -= TurnOnBlockFacing;
            eventChannel.OnAttackEnded -= TurnOffBlockFacing;
        }
    }
    public void UpdateDirection()
    {
        if (move.XVelocity > 0.01f && !IsFacingRight)
        {
            IsFacingRight = true;
            isFañingRight = IsFacingRight;
            rotate.eulerAngles = Vector3.zero;
            cameraFollowObject?.CallTurn();
        }
        else if (move.XVelocity < -0.01f && IsFacingRight)
        {
            IsFacingRight = false;
            IsFacingRight = IsFacingRight;
            rotate.eulerAngles = new Vector3(0, 180, 0);
            cameraFollowObject?.CallTurn();
        }
    }
    private void FixedUpdate()
    {
        if (!blockFacing)
            UpdateDirection();
        if (isCameraFollow)
        {
            UpdateYCameraDamping();
        }
    }

    private void UpdateYCameraDamping()
    {
        if (move.YVelocity < fallSpeedYDampingChangeThreshold &&
            !CameraManager.Instance.IsLerpingYDamping &&
            !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            //Debug.Log("[CharacterFacing] Detected falling. Triggering camera lerp to fall state.");
            CameraManager.Instance.LerpYDamping(true);
        }

        if (move.YVelocity >= 0 &&
            !CameraManager.Instance.IsLerpingYDamping &&
            CameraManager.Instance.LerpedFromPlayerFalling)
        {
            //Debug.Log("[CharacterFacing] Falling ended. Triggering camera return to normal.");
            CameraManager.Instance.LerpedFromPlayerFalling = false;
            CameraManager.Instance.LerpYDamping(false);
        }
    }

    private void TurnOnBlockFacing()
    {
        Debug.Log("TurnOnBlockFacing");
        blockFacing = true;
    }
    private void TurnOffBlockFacing()
    {
        Debug.Log("TurnOffBlockFacing");
        blockFacing = false;
    }
}
