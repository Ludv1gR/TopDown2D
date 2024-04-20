using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Vector2 movementInput;
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
    }

    void Update() {
        timeLastHit += Time.deltaTime;
    }

    private void Move() {
        // AI moving
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

    public void Defeated() {
        animator.SetTrigger("Defeated");
        canMove = false;
    }

    public void RemoveEnemy() {
        Destroy(gameObject);
    }
}
