using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float followDistance = 5f;
    public float height = 2f;
    public float followSpeed = 10f;

    private Vector3 targetPosition;

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 behindPlayer = playerTransform.position - playerTransform.forward * followDistance;
            targetPosition = new Vector3(behindPlayer.x, playerTransform.position.y + height, behindPlayer.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.LookAt(playerTransform.position + Vector3.up * height);
        }
    }
}
