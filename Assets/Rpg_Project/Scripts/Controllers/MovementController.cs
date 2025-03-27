using UnityEngine;

public class MovementController : MonoBehaviour
{
    private CharacterController characterController;
    public float moveSpeed = 5f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction)
    {
        if (direction.magnitude > 1f) direction.Normalize(); 
        characterController.Move(direction * moveSpeed * Time.deltaTime);
    }
}
