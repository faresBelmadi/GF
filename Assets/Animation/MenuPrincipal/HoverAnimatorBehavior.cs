using UnityEngine;

public class HoverAnimator : MonoBehaviour
{
    private Animator animator;
    private bool inBState = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseEnter()
    {
        if (!inBState)
            animator.SetTrigger("hover");
    }

    void OnMouseExit()
    {
        animator.SetTrigger("unhover");
    }

    // Optional: track when in B state
    public void SetBIdleState()
    {
        inBState = true;
    }
}
