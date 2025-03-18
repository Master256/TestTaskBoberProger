using UnityEngine;

public class GameInput : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetRotateCamera()
    {
        Vector2 inputVector = inputActions.Player.Look.ReadValue<Vector2>();

        return inputVector;
    }

    public bool IsInteractPressed()
    {
        return inputActions.Player.Interact.WasPressedThisFrame();
    }
}