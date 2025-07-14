using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayState(PlayerState state)
    {
        animator.Play(state.ToString());
    }
}
