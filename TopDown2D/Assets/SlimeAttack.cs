using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{   
    public Collider2D slimeCollider;
    public float damage = 10; // not actual value

    private void OnTriggerEnter2D(Collider2D players) { // Dont need attack function, this collider always on and in center (bodyslam)
        if (players.tag == "Player") {
            // deal damage to it
            PlayerController player = players.GetComponent<PlayerController>();

            if (player != null) {
                player.TakeDamage(damage);
                print("Taken damage");
            }
        }
    }
}
