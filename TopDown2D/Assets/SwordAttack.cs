using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    Vector2 frontAttackOffset;
    Vector2 rightAttackOffset = new (0.1f, -0.1f);
    Vector2 backAttackOffset = new(0.0f, -0.03f);
    Collider2D swordCollider;


    private void Start() {
        swordCollider = GetComponent<Collider2D>();
        frontAttackOffset = transform.position;
    }

    public void AttackRight() {
        print("Attack Right");
        swordCollider.enabled = true;
        transform.position = rightAttackOffset;
    }

    public void AttackLeft() {
        print("Attack Left");
        swordCollider.enabled = true;
        transform.position = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void AttackDown() {
        print("Attack Down");
        swordCollider.enabled = true;
        transform.position = frontAttackOffset;
    }

    public void AttackUp() {
        print("Attack Up");
        swordCollider.enabled = true;
        transform.position = backAttackOffset;
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            // deal damage to it
        }
    }
}
