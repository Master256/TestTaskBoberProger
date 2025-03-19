using UnityEngine;

public class RotateCameraPlayer : MonoBehaviour
{
    [SerializeField] private float Sensivity = 1.0f; // ����� ����
    [SerializeField] private float MaxYAngle = 80.0f; //������������ �������
    [SerializeField] private GameInput gameInput;

    private float RotationX; // ������� �� ��� X

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 lookInput = gameInput.GetRotateCamera();

        // ��������� ������� ������
        float mouseX = lookInput.x * Sensivity;
        float mouseY = lookInput.y * Sensivity;

        transform.parent.Rotate(eulers: Vector3.up * mouseX * Sensivity);

        RotationX -= mouseY * Sensivity;
        RotationX = Mathf.Clamp(value: RotationX, min: -MaxYAngle, MaxYAngle);
        transform.localRotation = Quaternion.Euler(RotationX, y: 0.0f, z: 0.0f);
    }

    public void SetSensivityCamera()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Sensivity = 0f;
    }
}