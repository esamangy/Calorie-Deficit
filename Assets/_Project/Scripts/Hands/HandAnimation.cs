using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for left mouse click
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            animator.SetTrigger("Left Click");
        }

        // Check for right mouse click
        if (Input.GetMouseButtonDown(1)) // 1 is the right mouse button
        {
            animator.SetTrigger("Right Click");
        }
    }
}
