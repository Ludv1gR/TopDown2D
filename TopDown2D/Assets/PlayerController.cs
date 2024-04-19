using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{   
    private enum LastDirection {
        side, down, up
    }

    private LastDirection lastDirection = LastDirection.down;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if (canMove) {
            // If movement input is not 0, try to move
            if (movementInput != Vector2.zero) {
                
                bool success = TryMove(movementInput);

                if (!success) {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }
                if (!success) {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }

                animator.SetBool("isMoving", success);
            } else {
                animator.SetBool("isMoving", false);
            }

            // Set direction of Sprite to movement direction
            // Sides
            if (movementInput.x < 0) {
                spriteRenderer.flipX = true;
            } else if (movementInput.x > 0) {
                spriteRenderer.flipX = false;
            }
            if (movementInput.x != 0 && Math.Abs(movementInput.x) >= Mathf.Abs(movementInput.y)) {
                animator.SetBool("sideMove", true);
                lastDirection = LastDirection.side;
            }
            if (movementInput.x == 0 || Math.Abs(movementInput.x) < Mathf.Abs(movementInput.y)) {
                animator.SetBool("sideMove", false);
            }

            // Back
            if (movementInput.y > 0 && movementInput.y > Mathf.Abs(movementInput.x)) {
                animator.SetBool("backMove", true);
                lastDirection = LastDirection.up;
            }
            if (movementInput.y <= 0 || Mathf.Abs(movementInput.y) <= Mathf.Abs(movementInput.x)) {
                animator.SetBool("backMove", false);
            }
            
            // Front
            if (movementInput.y < 0 && Mathf.Abs(movementInput.y) > Mathf.Abs(movementInput.x)) {
                animator.SetBool("frontMove", true);
                lastDirection = LastDirection.down;
            }
            if (movementInput.y >= 0 || Mathf.Abs(movementInput.y) <= Mathf.Abs(movementInput.x)) {
                animator.SetBool("frontMove", false); 
            }
        }
    }

    private bool TryMove(Vector2 direction) {
        if (direction != Vector2.zero) {
            // Check for potential collisions
            int count = rb.Cast(
                    direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                    movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                    castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                    moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount of cast equal to movement plus an offset
                
            if (count == 0) {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    void OnMove(InputValue movementValue) 
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack() {
        LockMovement();
        if (animator.GetBool("sideMove") || lastDirection == LastDirection.side) {
            if (spriteRenderer.flipX == true) {
                swordAttack.AttackLeft();
            } else {
                swordAttack.AttackRight();
            }
        }
        if (animator.GetBool("backMove") || lastDirection == LastDirection.up) {
            swordAttack.AttackUp();
        }
        if (animator.GetBool("frontMove") || lastDirection == LastDirection.down) {
            swordAttack.AttackDown();
        }
    }

    public void StopSwordAttackHitbox() {
        swordAttack.StopAttack();
    }

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }
}
