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

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(this);
        }
    }

    public void SetValues(string enemyName, int[] pointValues, int[] timeValues, Sprite[] sprites, float gravityScale, float mass, int noReturnBounderies)
    {
        this.enemyName = enemyName;
        this.pointValues = pointValues;
        this.timeValues = timeValues;
        this.sprites = sprites;
        this.gravityScale = gravityScale;
        this.mass = mass;
        this.noReturnBounderies = noReturnBounderies;
    }

    public void UpdateTime(float time)
    {
        timeChange = timeChange + (int)time; //update our time
        int curStage = 0; //set our current stage to 0


        // Check if the time is above or below the boundaries
        if (timeChange < timeValues[0] - noReturnBounderies)
        {
            isBelowBoundary = true;
            destroyEnemy();
            return;
        }
        else if (timeChange > timeValues[timeValues.Length - 1] + noReturnBounderies)
        {
            isAboveBoundary = true;
            destroyEnemy();
            return;
        }

        // Find the stage that corresponds to the current time
        for (int i = 0; i < timeValues.Length; i++)
        {
            curStage = i; //set our current stage to i incase we are greater then our final index so that way we set our stage to the final index, if we are less then a value we will hit the break and exit the loop
            if (timeChange <= timeValues[i])
            {
                break;
            }
        }
        //timechange = 105 and timevalues[0,25,50,75,100]  - currentstage = 4
        // Check if the time falls between two values
        if (curStage > 0 && timeChange < timeValues[curStage] && timeChange > timeValues[curStage - 1])
        {
            curStage--; // set our current stage to the previous index since we are rounding down to a stage that is less then our current time
        }

        //update our current stage
        currentStage = curStage;

        // Update the sprite of this enemy
        this.GetComponent<SpriteRenderer>().sprite = sprites[currentStage];
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

        Destroy(this);
    }
}
