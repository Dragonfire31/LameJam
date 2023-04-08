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
    [SerializeField] private float gravityScale; // gravity scale for specific enemy, may be heavier for larger objects and lighter for smaller objects
    [SerializeField] private float mass; // mass for specific enemy, may be heavier for larger objects and lighter for smaller objects
    [SerializeField] private int noReturnBounderies; //time change to be added and subtracted from the top and bottom of the array, to low object will disapear from time, to large object turn into goop enemy
    [SerializeField] private GameObject enemyGameObject; // enemy object
    private int timeChange; // time tracker for object after spawning to figure out what stage/ index of the array it is in
    GameObject enemy; // enemy object
    private int currentStage; //current stage of the enemy

    public void SpawnEnemy(Vector2 position)
    {

        // Choose a random stage
        int stage = Random.Range(0, pointValues.Length);
        currentStage = stage;
        
        //set our time to a starting value from random stage using timeValues
        timeChange = timeValues[currentStage];


        // Instantiate enemy with chosen sprite
        enemy = enemyGameObject;
        enemy.transform.position = position;
        //enemy.AddComponent<SpriteRenderer>().sprite = sprites[currentStage];
        enemy.GetComponent<SpriteRenderer>().sprite = sprites[currentStage];


        // Set rigidbody properties
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.mass = mass;

        //
        Instantiate(enemy);
        Debug.Log(enemyName+" Created");

    }


}



