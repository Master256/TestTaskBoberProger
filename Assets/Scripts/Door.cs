using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Inventory inventory;
    [SerializeField] private RotateCameraPlayer rotateCameraPlayer;
    [SerializeField] private MovementPlayer movementPlayer;
    [SerializeField] private GameObject victoryUI;

    private bool isInteractable = false;

    private void Awake()
    {
        victoryUI.SetActive(false);
    }

    private void Update()
    {
        RaycastHit hit;
        bool isPlayerNear = Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, playerLayer);

        if (isPlayerNear)
        {
            ItemSO currentItem = inventory.GetCurrentItem();
            if (currentItem != null && currentItem.itemName == "Key")
            {
                isInteractable = true;
            }
            else
            {
                isInteractable = false;
            }
        }
        else
        {
            isInteractable = false;
        }

        if (isInteractable && Input.GetKeyDown(KeyCode.F))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }

        rotateCameraPlayer.SetSensivityCamera();
        movementPlayer.SetSpeedPlayer();

        Time.timeScale = 0f;
    }
}