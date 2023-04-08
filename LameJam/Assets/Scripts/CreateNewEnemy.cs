using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Create New Enemy")]
public class CreateNewEnemy : ScriptableObject
{
    [SerializeField] private string enemyName; //Name of the enemy
    [SerializeField] private int[] pointValues; // points for each array index
    [SerializeField] private int[] timeValues; // time for each array index
    [SerializeField] private Sprite[] sprites; // sprites for each array index
    [SerializeField] private Sprite gloopSprite; // sprite for gloop enemy
    [SerializeField] private float gravityScale; // gravity scale for specific enemy, may be heavier for larger objects and lighter for smaller objects
    [SerializeField] private float mass; // mass for specific enemy, may be heavier for larger objects and lighter for smaller objects
    [SerializeField] private int noReturnBounderies; //time change to be added and subtracted from the top and bottom of the array, to low object will disapear from time, to large object turn into goop enemy
    [SerializeField] private GameObject enemyGameObject; // enemy object
    private int timeChange; // time tracker for object after spawning to figure out what stage/ index of the array it is in
    GameObject enemy; // enemy object
    private int currentStage; //current stage of the enemy
    private int startingValue;
    [SerializeField] private float ageChangeSpeed = 1.0f;

    public void SpawnEnemy(Vector2 position)
    {
        // Choose a random stage
        float age = Random.Range(timeValues[0], timeValues[timeValues.Length - 1]);

        int currentStage = 0;
        for (int i = 0; i < timeValues.Length - 1; i++)
        {
            if (age > timeValues[i] && age <= timeValues[i + 1])
            {
                currentStage = i + 1;
                break;
            }
        }

        // Instantiate enemy with chosen sprite
        enemy = enemyGameObject;
        enemy.transform.position = position;
        enemy.GetComponent<SpriteRenderer>().sprite = sprites[currentStage];
        startingValue = pointValues[currentStage];

        // Set rigidbody properties
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.mass = mass;

        // Instantiate the enemy object
        GameObject instantiatedEnemy = Instantiate(enemy);
        instantiatedEnemy.AddComponent<PolygonCollider2D>();

        Debug.Log("Value: " + startingValue);
        // Call SetValues on the instantiated enemy object
        instantiatedEnemy.GetComponent<EnemyBehavior>().SetValues(enemyName, pointValues, timeValues, sprites, gravityScale, mass, noReturnBounderies, age, startingValue, ageChangeSpeed, gloopSprite);

        // Set the layer mask to layer 7
        instantiatedEnemy.layer = 7;
    }


}



