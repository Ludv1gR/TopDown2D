using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 4; // not actual value
    Vector2 frontAttackOffset;
    Vector2 rightAttackOffset = new (0.1f, -0.1f);
    Vector2 backAttackOffset = new(0.0f, -0.03f);

    private void Start() {
        frontAttackOffset = transform.position;
    }

    public void AttackRight() {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void AttackDown() {
        swordCollider.enabled = true;
        transform.localPosition = frontAttackOffset;
    }

    public void AttackUp() {
        swordCollider.enabled = true;
        transform.localPosition = backAttackOffset;
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            // deal damage to it
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
        }
    }
}
