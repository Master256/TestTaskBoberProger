using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 3f;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private Transform handTransform;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Hotbar hotbar;

    private GameObject currentHeldItem;
    private Vector3[] heldItemOriginalScale;
    private Vector3[] heldItemColliderSize;
    private Vector3[] heldItemColliderCenter;
    private ItemSO[] heldItemSO;
    private bool isHoldingItem = false;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory component not found on the player!");
        }
    }

    private void Start()
    {
        heldItemOriginalScale = new Vector3[hotbar.GetLengthSlots()];
        heldItemColliderSize = new Vector3[hotbar.GetLengthSlots()];
        heldItemColliderCenter = new Vector3[hotbar.GetLengthSlots()];
        heldItemSO = new ItemSO[hotbar.GetLengthSlots()];
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
                int index = hotbar.GetSelectedSlot();

                heldItemSO[index] = itemWorld.GetItem();
                heldItemOriginalScale[index] = itemWorld.GetOriginalScale();

                BoxCollider itemCollider = hit.collider.GetComponent<BoxCollider>();
                if (itemCollider != null)
                {
                    heldItemColliderSize[index] = itemCollider.size;
                    heldItemColliderCenter[index] = itemCollider.center;
                }
                else
                {
                    heldItemColliderSize[index] = Vector3.one;
                    heldItemColliderCenter[index] = Vector3.zero;
                }

                inventory.AddItem(heldItemSO[index]);
                DisplayItemInHand(heldItemSO[index], heldItemOriginalScale[index], heldItemColliderSize[index], heldItemColliderCenter[index]);

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

            int slotIndex = hotbar.GetSelectedSlot();
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

                droppedItem.transform.localScale = heldItemOriginalScale[slotIndex];

                BoxCollider boxCollider = droppedItem.GetComponent<BoxCollider>();
                if (boxCollider == null)
                {
                    boxCollider = droppedItem.AddComponent<BoxCollider>();
                }

                boxCollider.size = heldItemColliderSize[slotIndex];
                boxCollider.center = heldItemColliderCenter[slotIndex];

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

                itemWorld.SetItem(heldItemSO[slotIndex]);

                inventory.RemoveItem(slotIndex);
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

    public void SetActiveObject()
    {
        if (currentHeldItem != null)
        {
            Destroy(currentHeldItem);
            currentHeldItem = null;
            isHoldingItem = false;
        }

        int currentSlotIndex = hotbar.GetSelectedSlot();

        if (inventory.HasItemInSlot(currentSlotIndex))
        {
            ItemSO currentItem = inventory.GetCurrentItem();

            if (currentItem != null && currentItem.prefab != null)
            {
                currentHeldItem = Instantiate(currentItem.prefab, handTransform);
                currentHeldItem.transform.localPosition = Vector3.zero;
                currentHeldItem.transform.localRotation = Quaternion.identity;
                currentHeldItem.transform.localScale = heldItemOriginalScale[currentSlotIndex];

                isHoldingItem = true;
            }
        }
    }
}