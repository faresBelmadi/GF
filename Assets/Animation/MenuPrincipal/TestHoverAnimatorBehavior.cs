using UnityEngine;

public class HoverAnimator : MonoBehaviour
{
    private Animator animator;
    private bool inBState = false;
    private bool clicked = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseEnter()
    {
        if (!clicked && !inBState)
        {
            animator.SetTrigger("CharacterHover");
        }
    }

    void OnMouseExit()
    {
        animator.SetTrigger("CharacterBack");
    }

     void OnMouseDown()
    {
        if (!clicked)
        {
            clicked = true;
            animator.SetTrigger("CharacterClick");
        }
    }

    // Optional: track when in B state
    public void SetBIdleState()
    {
        inBState = true;
    }
}
