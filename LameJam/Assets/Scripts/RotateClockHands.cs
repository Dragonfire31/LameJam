using UnityEngine;

public class RotateClockHands : MonoBehaviour
{
    [SerializeField] private bool isHourHand;
    [SerializeField] private GameObject otherHand;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float maxSpeed = 50f;

    private Quaternion startRotation;
    private Quaternion otherHandStartRotation;
    private float totalRotation;
    private bool isPaused;

    private void Start()
    {
        startRotation = transform.rotation;
        otherHandStartRotation = otherHand.transform.rotation;
    }

    private void Update()
    {
        if (!isPaused)
        {
            float rotationInput = 0;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rotationInput = 1;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.RightArrow))
            {
                rotationInput = -1;
            }

            if (rotationInput != 0)
            {
                float angleDifference = maxSpeed * Time.deltaTime * rotationInput;
                totalRotation += angleDifference;

                transform.rotation = Quaternion.Euler(0, 0, startRotation.eulerAngles.z + totalRotation);
                UpdateOtherHandRotation(totalRotation);
            }
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

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }
}
