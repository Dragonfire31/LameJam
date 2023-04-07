using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Create New Enemy")]
public class CreateNewEnemy : ScriptableObject
{
    [SerializeField] private int[] pointValues;
    [SerializeField] private int[] timeValues;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float gravityScale;
    [SerializeField] private float mass;
    [SerializeField] private float spawnTime;
    [SerializeField] private int numberOfStages;

    public void SpawnEnemy(Vector2 position)
    {
        // Choose a random stage
        int stage = Random.Range(0, numberOfStages);

        // Instantiate enemy with chosen sprite
        GameObject enemy = new GameObject("Enemy");
        enemy.transform.position = position;
        enemy.AddComponent<SpriteRenderer>().sprite = sprites[stage];

        // Add a script to control the enemy's movement
        //enemy.AddComponent<EnemyMovement>().PointValue = pointValues[stage];
        //enemy.AddComponent<EnemyMovement>().TimeToLive = timeValues[stage];

        //Add script select a random index for arrays


        // Set rigidbody properties
        Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.mass = mass;

        // Add a collider
        enemy.AddComponent<BoxCollider2D>();

    }
}



