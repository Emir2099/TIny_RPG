using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Assign your player in the Inspector
    public float smoothSpeed = 0.125f;
    public Vector3 offset;     // Optional: to position the camera slightly above or behind the player

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
