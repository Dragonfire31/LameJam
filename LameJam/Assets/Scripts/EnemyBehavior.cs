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
    private float ageChangeSpeed;
    public Sprite gloopSprite; // sprite for gloop enemy

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<Rigidbody2D>();
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

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            age -= Time.deltaTime * ageChangeSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            age += Time.deltaTime * ageChangeSpeed;
        }

        UpdateCurrentStage();
    }

    public void SetValues(string enemyName, int[] pointValues, int[] timeValues, Sprite[] sprites, float gravityScale, float mass, int noReturnBounderies, float age, int currentValue, float ageChangeSpeed, Sprite gloopSprite)
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
        this.ageChangeSpeed = ageChangeSpeed;
        this.gloopSprite = gloopSprite;
    }

    public void scoreEnemy()
    {
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

    private void UpdateCurrentStage()
    {
    
        int highestTimeValue = timeValues[timeValues.Length - 1];

        if (age >= noReturnBounderies)
        {
            GetComponent<SpriteRenderer>().sprite = gloopSprite;
            currentValue = -50;
            return;
        }

        for (int i = 0; i < timeValues.Length; i++)
        {
            if (age >= timeValues[i] && age < (i + 1 < timeValues.Length ? timeValues[i + 1] : float.MaxValue))
            {
                if (currentStage != i)
                {
                    currentStage = i;
                    UpdatePointsAndSprite();
                }
                break;
            }
        }
    
    }


    private void UpdatePointsAndSprite()
    {
        currentValue = pointValues[currentStage];
        GetComponent<SpriteRenderer>().sprite = sprites[currentStage];
        UpdatePolygonCollider();
    }

    private void UpdatePolygonCollider()
    {
        DestroyImmediate(GetComponent<PolygonCollider2D>());
        this.gameObject.AddComponent<PolygonCollider2D>();
    }

    public void getGlooped()
    {
        // Remove collider and rigidbody
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());

        // Gradually scale the object up to 100x its size
        GetComponent<SpriteRenderer>().sortingOrder = 50;
        StartCoroutine(ScaleUp());

        // Add negative score to game manager
        gameManager.GetComponent<GameEventHandler>().AddScore(-50);
    }

    private IEnumerator ScaleUp()
    {
        float scaleIncrement = 0.3f; // The amount by which to increase the scale on each frame
        Vector3 originalScale = transform.localScale; // The object's original scale
        Vector3 targetScale = originalScale * 100f; // The scale we want to reach

        while (transform.localScale.x < targetScale.x)
        {
            // Increase the scale by scaleIncrement
            transform.localScale += new Vector3(scaleIncrement, scaleIncrement, 0f);

            // Wait for the next frame
            yield return null;
        }

        // Destroy the object
        Destroy(gameObject);
    }
}
