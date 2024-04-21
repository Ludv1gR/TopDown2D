using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
    public bool playerInRange;
    public float playerInRangeDistance = 0.5f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    bool canMove = true;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // TryMove (senare)
        spriteRenderer = GetComponent<SpriteRenderer>(); // flipX när den vänder sig (senare)
        playerTarget = GameObject.Find("Player").transform;
        playerInRange = false;
        movementFilter.useLayerMask = true;
        movementFilter.layerMask = ~(1 << 3 | 1 << 6); // Ignore collisions between layers 3 and 6
    }

    void Update() {
        timeLastHit += Time.deltaTime;
        if (playerTarget) {
            Vector3 enemyToPlayerVector = playerTarget.position - transform.position + new Vector3 (0f, -0.12f);
            Vector2 direction = enemyToPlayerVector.normalized;
            if (enemyToPlayerVector.magnitude <= playerInRangeDistance) {
                playerInRange = true;
                moveDirection = direction;
            } else {
                playerInRange = false;
                moveDirection = Vector2.zero;
            }
            
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
            if (playerInRange) {
                
                bool success = Move(moveDirection);

                if (!success) {
                    success = Move(new Vector2(moveDirection.x, 0));
                }
                if (!success) {
                        success = Move(new Vector2(0, moveDirection.y));
                    }

                animator.SetBool("isMoving", success);
            } else {
                Move(new Vector2(0, 0));
                animator.SetBool("isMoving", false);
            }
        } else {
            Move(new Vector2(0, 0));
            animator.SetBool("isMoving", false);
        }
    }

    private bool Move(Vector2 direction) {
        
        if (direction != Vector2.zero) {
            // Check for potential collisions
            int count = rb.Cast(
                    direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                    movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                    castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                    moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount of cast equal to movement plus an offset
                
            if (count == 0) {
                rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
                return true;
            } else {
                rb.velocity = Vector2.zero;
                return false;
            }
        } else {
            rb.velocity = Vector2.zero;
            return false;
        }


        //rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
        //return true;
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
