using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float pushForce = 3f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Camera playerCamera;

    private CharacterController characterController;
    private Vector3 verticalVelocity;
    private bool isGrounded;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
        }
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; // Прижатие игрока гравитацией
        }

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        moveDirection = playerCamera.transform.TransformDirection(moveDirection);
        moveDirection.y = 0; // Обнуление по вертикали

        moveDirection.Normalize();

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        verticalVelocity.y += gravity * Time.deltaTime; // Применение гравитации к игроку

        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRigidbody = hit.collider.attachedRigidbody;
        if (hitRigidbody != null && !hitRigidbody.isKinematic)
        {
            Vector3 pushDirection = hit.moveDirection;
            pushDirection.y = 0;

            hitRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}