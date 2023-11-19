using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveH = 0;
    private bool triggerChest;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        movement = new Vector2(moveX, moveY);
        animator.SetFloat("VelX", moveX);
        animator.SetFloat("VelY", moveY);
        animator.SetFloat("Speed", movement.magnitude);
        // Cập nhật hướng đối diện
        if (moveX > 0)
        {
            Flip(-1);
        }
        else if (moveX < 0)
        {
            Flip(1);
        }

    }

    private void FixedUpdate()
    {
        if (triggerChest)
            return;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    private void UpdateAnimationState()
    {
        if (movement != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    // Hàm đảo hướng nhân vật
    private void Flip(float flip)
    {
        transform.localScale = new Vector3(flip, transform.localScale.y);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouchingLayers(3))
        {
            triggerChest = true;
            Manager.Instance.TriggerChest();
        }
    }
}
