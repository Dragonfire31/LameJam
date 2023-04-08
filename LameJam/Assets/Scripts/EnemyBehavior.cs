using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private int timeChange; // time tracker for object after spawning to figure out what stage/ index of the array it is in
    private bool isBelowBoundary = false; //if below boundary, destroy enemy in puff of smoke
    private bool isAboveBoundary = false; //if above boundary, destroy enemy and spawn goop enemy
    private bool isOutsideBoundary = false; //if outside boundary, destroy enemy
    private int currentStage; //current stage of the enemy
    private string enemyName; //Name of the enemy
    private int[] pointValues; // points for each array index
    private int[] timeValues; // time for each array index
    private Sprite[] sprites; // sprites for each array index
    private float gravityScale; // gravity scale for specific enemy, may be heavier for larger objects and lighter for smaller objects
    private float mass; // mass for specific enemy, may be heavier for larger objects and lighter for smaller objects
    private int noReturnBounderies; //time change to be added and subtracted from the top and bottom of the array, to low object will disapear from time, to large object turn into goop enemy
    private float age; // age of the enemy
    private int currentValue;
    private bool valuesSet = false;
    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
       gameManager = GameObject.Find("GameManager");
       if(gameManager == null)
       {
           Debug.LogWarning("Game Manager not found");
       }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }


    }

    public void SetValues(string enemyName, int[] pointValues, int[] timeValues, Sprite[] sprites, float gravityScale, float mass, int noReturnBounderies, float age, int currentValue)
    {
        this.enemyName = enemyName;
        this.pointValues = pointValues;
        this.timeValues = timeValues;
        this.sprites = sprites;
        this.gravityScale = gravityScale;
        this.mass = mass;
        this.noReturnBounderies = noReturnBounderies;
        this.age = age;
        this.currentValue = currentValue;
        valuesSet = true;
        Debug.Log("Current Value: " + currentValue);
        }

    public void scoreEnemy()
    {
        if (!valuesSet)
        {
            Debug.LogWarning("Values not set yet");
            return;
        }

        //Check if gameOject exists
        if (this.gameObject != null)
        {
            Debug.Log("Score Value: " + currentValue);
            gameManager.GetComponent<GameEventHandler>().AddScore(currentValue);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.LogWarning("Enemy does not exist");
        }


    }

    public void destroyEnemy()
    {
        //If enemy is outside the boundary of the scene, destroy it, otherwise, run checks below for spawn goop enemy or smoke puff and add points to game manager
        if (!isOutsideBoundary)
        {
            // Check if the enemy is above or below the boundaries
            if (isAboveBoundary)
            {
                //spawn goop enemy
                //GameManager.Instance.SpawnGoopEnemy(enemy.transform.position);
            }
            else if (isBelowBoundary)
            {
                //spawn smoke puff
                //GameManager.Instance.SpawnSmokePuff(enemy.transform.position);
            }
            else if (!isAboveBoundary && !isBelowBoundary)
            {
                // add points to GameManager based on pointValues and current stage
                //send point value of pointValues[currentStage]
                // send back points to game manager
                //GameManager.Instance.AddPoints(enemy.GetComponent<EnemyMovement>().PointValue);
            }
        }

        Destroy(this.gameObject);
    }
}
