using UnityEngine;

public class RotateClockHands : MonoBehaviour
{
    [SerializeField] bool isHourHand;
    [SerializeField] GameObject otherHand;
    private bool isDragging;
    private Vector3 startMousePosition;
    private float startRotation;

    private void Update()
    {
        if (isDragging)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;

            Vector3 clockCenter = transform.position;
            Vector3 startMouseDirection = startMousePosition - clockCenter;
            Vector3 currentMouseDirection = currentMousePosition - clockCenter;

            float angleDifference = Vector3.SignedAngle(startMouseDirection, currentMouseDirection, Vector3.forward);
            transform.eulerAngles = new Vector3(0, 0, startRotation + angleDifference);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startMousePosition.z = 0;
        startRotation = transform.eulerAngles.z;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
