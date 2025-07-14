using UnityEngine;

public class PlayerAnimationStateResolver : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimationController animationController;

    private PlayerState currentAnimationState;

    private void Update()
    {
        var newState = ResolveAnimationState();

        if (newState != currentAnimationState)
        {
            currentAnimationState = newState;
            animationController.PlayState(currentAnimationState);
        }
    }

    private PlayerState ResolveAnimationState()
    {
        if (playerMovement.isDashing)
            return PlayerState.Dash;

        if ((playerMovement.isTouchingLeftWall || playerMovement.isTouchingRightWall) && !playerMovement.isGrounded && playerMovement.YVelocity < 0)
            return PlayerState.WallSlide;

        if (!playerMovement.isGrounded)
        {
            if (playerMovement.YVelocity > 0.1f)
                return PlayerState.Jump;
            else if (playerMovement.YVelocity < -0.1f)
                return PlayerState.Fall;
        }

        if (Mathf.Abs(playerMovement.XVelocity) > 0.1f)
            return PlayerState.Run;

        return PlayerState.Idle;
    }
}
