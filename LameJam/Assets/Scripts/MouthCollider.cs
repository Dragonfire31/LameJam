using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBehavior enemyBehavior = other.gameObject.GetComponent<EnemyBehavior>();
        if (enemyBehavior == null)
        {
            Debug.LogWarning($"Enemy behavior not found on {other.gameObject.name}. Check if the script is attached.");
        }
        else
        {
            SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite == enemyBehavior.gloopSprite)
            {
                // Gloop enemy behavior
                Debug.Log($"Gloop enemy {other.gameObject.name} collided with the mouth.");
                other.GetComponent<EnemyBehavior>().getGlooped();
            }
            else if (other.gameObject.layer == 7)
            {
                // Normal enemy behavior
                other.GetComponent<EnemyBehavior>().scoreEnemy();
            }
            else
            {
                Debug.LogWarning($"Object {other.gameObject.name} collided with the mouth, but its layer is not 7.");
            }
        }
    }

}
