using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 3f;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private Transform handTransform;
    [SerializeField] private GameInput gameInput;

    private Inventory inventory;
    private GameObject currentHeldItem;
    private Vector3 heldItemOriginalScale;
    private Vector3 heldItemColliderSize;
    private Vector3 heldItemColliderCenter;
    private ItemSO heldItemSO;
    private bool isHoldingItem = false;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory component not found on the player!");
        }
    }

    private void Update()
    {
        if (gameInput.IsInteractPressed())
        {
            if (isHoldingItem)
            {
                DropItem();
            }
            else
            {
                TryPickupItem();
            }
        }
    }

    private void TryPickupItem()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupDistance, itemLayer);

        if (isHit)
        {
            ItemWorld itemWorld = hit.collider.GetComponent<ItemWorld>();
            if (itemWorld != null)
            {
                heldItemSO = itemWorld.GetItem();
                heldItemOriginalScale = itemWorld.GetOriginalScale();

                BoxCollider itemCollider = hit.collider.GetComponent<BoxCollider>();
                if (itemCollider != null)
                {
                    heldItemColliderSize = itemCollider.size;
                    heldItemColliderCenter = itemCollider.center;
                }
                else
                {
                    heldItemColliderSize = Vector3.one;
                    heldItemColliderCenter = Vector3.zero;
                }

                inventory.AddItem(heldItemSO);

                DisplayItemInHand(heldItemSO, heldItemOriginalScale, heldItemColliderSize, heldItemColliderCenter);

                Destroy(hit.collider.gameObject);

                isHoldingItem = true;
            }
        }
    }

    private void DisplayItemInHand(ItemSO item, Vector3 originalScale, Vector3 colliderSize, Vector3 colliderCenter)
    {
        if (item.prefab != null)
        {
            currentHeldItem = Instantiate(item.prefab, handTransform);
            currentHeldItem.transform.localPosition = Vector3.zero;
            currentHeldItem.transform.localRotation = Quaternion.identity;
            currentHeldItem.transform.localScale = originalScale;

            BoxCollider boxCollider = currentHeldItem.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = currentHeldItem.AddComponent<BoxCollider>();
            }

            boxCollider.size = colliderSize;
            boxCollider.center = colliderCenter;
        }
    }

    private void DropItem()
    {
        if (currentHeldItem != null)
        {
            Destroy(currentHeldItem);

            ItemSO item = inventory.GetCurrentItem();
            if (item != null && item.prefab != null)
            {
                Vector3 spawnPosition = handTransform.position + handTransform.forward * 2;

                GameObject droppedItem = Instantiate(item.prefab, spawnPosition, Quaternion.identity);
                if (droppedItem == null)
                {
                    return;
                }

                int itemLayerIndex = GetFirstLayerIndex(itemLayer);
                if (itemLayerIndex != -1)
                {
                    droppedItem.layer = itemLayerIndex;
                }

                droppedItem.transform.localScale = heldItemOriginalScale;

                BoxCollider boxCollider = droppedItem.GetComponent<BoxCollider>();
                if (boxCollider == null)
                {
                    boxCollider = droppedItem.AddComponent<BoxCollider>();
                }

                boxCollider.size = heldItemColliderSize;
                boxCollider.center = heldItemColliderCenter;

                Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = droppedItem.AddComponent<Rigidbody>();
                }

                rb.mass = 1f;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                ItemWorld itemWorld = droppedItem.GetComponent<ItemWorld>();
                if (itemWorld == null)
                {
                    itemWorld = droppedItem.AddComponent<ItemWorld>();
                }

                itemWorld.SetItem(heldItemSO);

                inventory.RemoveItem(item);
            }

            isHoldingItem = false;
        }
    }

    /// <summary>
    /// Метод для получения номера слоя
    /// </summary>
    private int GetFirstLayerIndex(LayerMask layerMask)
    {
        int layerValue = layerMask.value;
        if (layerValue == 0)
        {
            return -1;
        }

        for (int i = 0; i < 32; i++)
        {
            if ((layerValue & (1 << i)) != 0)
            {
                return i;
            }
        }

        return -1;
    }
}