using UnityEngine;

public class RotateClockHands : MonoBehaviour
{
    [SerializeField] private bool isHourHand;
    [SerializeField] private GameObject otherHand;
    [SerializeField] private LayerMask collisionMask;

    private bool isDragging;
    private Vector3 startMousePosition;
    private Quaternion startRotation;
    private Quaternion otherHandStartRotation;
    private float totalRotation;

    private void Update()
    {
        if (isDragging)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;

            Vector3 clockCenter = transform.position;
            Vector3 startMouseDirection = (startMousePosition - clockCenter).normalized;
            Vector3 currentMouseDirection = (currentMousePosition - clockCenter).normalized;

            Quaternion startQuaternion = Quaternion.LookRotation(Vector3.forward, startMouseDirection);
            Quaternion currentQuaternion = Quaternion.LookRotation(Vector3.forward, currentMouseDirection);
            Quaternion rotationDelta = currentQuaternion * Quaternion.Inverse(startQuaternion);

            float angleDifference = rotationDelta.eulerAngles.z;
            if (angleDifference > 180f)
            {
                angleDifference -= 360f;
            }
            totalRotation += angleDifference;

            if (!CheckForCollision(angleDifference))
            {
                transform.rotation = rotationDelta * startRotation;
                UpdateOtherHandRotation(totalRotation);
            }

            startMousePosition = currentMousePosition;
            startRotation = transform.rotation;
        }
    }

    private void UpdateOtherHandRotation(float totalRotation)
    {
        if (isHourHand)
        {
            // This is the hour hand, so the other hand is the minute hand
            otherHand.transform.rotation = Quaternion.Euler(0, 0, totalRotation * 12) * otherHandStartRotation;
        }
        else
        {
            // This is the minute hand, so the other hand is the hour hand
            otherHand.transform.rotation = Quaternion.Euler(0, 0, totalRotation / 12) * otherHandStartRotation;
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startMousePosition.z = 0;
        startRotation = transform.rotation;
        otherHandStartRotation = otherHand.transform.rotation;
        totalRotation = 0;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

        private bool CheckForCollision(float angleDifference)
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = Quaternion.Euler(0, 0, angleDifference) * startPoint;

        RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint, collisionMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }
}
