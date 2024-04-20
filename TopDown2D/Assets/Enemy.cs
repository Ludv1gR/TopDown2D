using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Vector2 moveDirection;
    Transform playerTarget;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public SlimeAttack slimeAttack;
    
    public float health = 10; // not acutal value
    public float damage = 5;
    float timeLastHit = 0.0f;
    public float moveSpeed = 0.8f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    bool canMove = true;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // TryMove (senare)
        spriteRenderer = GetComponent<SpriteRenderer>(); // flipX när den vänder sig (senare)
        playerTarget = GameObject.Find("Player").transform;
    }

    void Update() {
        timeLastHit += Time.deltaTime;
        if (playerTarget) {
            Vector3 direction = (playerTarget.position - transform.position + new Vector3 (0f, -0.12f)).normalized;
            moveDirection = direction;
            if (direction.x < 0) {
                spriteRenderer.flipX = true;
            } else if (direction.x > 0) {
                spriteRenderer.flipX = false;
            }
        }
    }

    void FixedUpdate() {
        if (canMove) {
            // If movement input is not 0, try to move
            if (moveDirection != Vector2.zero) {
                
                bool success = Move(moveDirection);

                if (!success) {
                    success = Move(new Vector2(moveDirection.x, 0));
                }
                if (!success) {
                        success = Move(new Vector2(0, moveDirection.y));
                    }

                animator.SetBool("isMoving", success);
            } else {
                animator.SetBool("isMoving", false);
            }
        }
    }

    private bool Move(Vector2 direction) {
        rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
        return true;
    }

    public void TakeDamage(float damage) {
        if (timeLastHit > 0.35f) {
            timeLastHit = 0.0f;
            health -= damage;
            animator.SetTrigger("Hit");

            if (health <= 0) {
                Defeated(); 
            }
        }
    }

    public void CantMove() {
        canMove = false;
    }

    public void CanMove() {
        canMove = true;
    }

    public void Defeated() {
        animator.SetTrigger("Defeated");
        canMove = false;
    }

    public void RemoveEnemy() {
        Destroy(gameObject);
    }
}
