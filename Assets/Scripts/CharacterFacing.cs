using UnityEngine;

public class CharacterFacing : MonoBehaviour
{
    public Vector2 FacingDirection { get; private set; } = Vector2.right;

    public void UpdateDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0.01f)
            FacingDirection = Vector2.right;
        else if (moveInput.x < -0.01f)
            FacingDirection = Vector2.left;
    }
}
