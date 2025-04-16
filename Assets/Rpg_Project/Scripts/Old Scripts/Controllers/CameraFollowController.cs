using UnityEngine;

public class CameraFollowController : MonoBehaviour, ICameraMovement
{
    public float followDistance = 10f;
    public float height = 5f;
    public Transform target;
    private Vector3 currentVelocity;

    public void FollowPlayer(Transform playerTransform)
    {
        Vector3 targetPosition = playerTransform.position + Vector3.up * height - playerTransform.forward * followDistance;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.1f);
    }
}
