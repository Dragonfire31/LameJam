using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
    public LayerMask layerMask = -1; // Make sure we aren't in this layer
    public float skinWidth = 0.1f; // Probably doesn't need to be changed

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;

    // Initialize values
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    void FixedUpdate()
    {
        // Have we moved more than our minimum extent?
        Vector3 currentPosition = myRigidbody.position;
        Vector3 movementThisStep = currentPosition - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit2D hitInfo = Physics2D.Raycast(previousPosition, movementThisStep, movementMagnitude, layerMask);

            // Check for obstructions we might have missed
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.isTrigger)
                    hitInfo.collider.SendMessage("OnTriggerEnter2D", myCollider);

                if (!hitInfo.collider.isTrigger)
                    myRigidbody.position = hitInfo.point - (Vector2)(movementThisStep / movementMagnitude) * partialExtent;
            }
        }

        previousPosition = currentPosition;
    }
}
