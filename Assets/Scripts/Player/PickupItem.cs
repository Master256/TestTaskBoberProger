using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 3f; // ��������� ������� ���������
    [SerializeField] private LayerMask itemLayer; // ���� ��� ���������
    [SerializeField] private Transform handTransform; // �����, ��� ����� ������������ ������� � �����
    [SerializeField] private GameInput gameInput; // ������ �� GameInput

    private Inventory inventory; // ������ �� ���������
    private GameObject currentHeldItem; // ������� ������� � �����
    private Vector3 heldItemOriginalScale; // ������������ ������ �������� � �����
    private Vector3 heldItemColliderSize; // ������������ ������ ����������
    private bool isHoldingItem = false; // ����, �����������, ������ �� ����� �������

    private void Awake()
    {
        inventory = GetComponent<Inventory>(); // �������� ��������� Inventory
        if (inventory == null)
        {
            Debug.LogError("Inventory component not found on the player!");
        }
    }

    private void Update()
    {
        // ���������, ������ �� ������� ��������������
        if (gameInput.IsInteractPressed())
        {
            if (isHoldingItem)
            {
                // ���� ����� ������ �������, ��������� ���
                DropItem();
            }
            else
            {
                // ���� ����� �� ������ �������, �������� ���������
                TryPickupItem();
            }
        }
    }

    private void TryPickupItem()
    {
        // ������� ��� ��� ����������� ���������
        RaycastHit hit;
        bool isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupDistance, itemLayer);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * pickupDistance, Color.red, 1f); // ������ ��� � �����

        if (isHit)
        {
            ItemWorld itemWorld = hit.collider.GetComponent<ItemWorld>();
            if (itemWorld != null)
            {
                Debug.Log($"������ �������: {itemWorld.name}");
                // ��������� ������� � ���������
                inventory.AddItem(itemWorld.GetItem());

                // ���������� ������� � �����
                DisplayItemInHand(itemWorld.GetItem(), itemWorld.GetOriginalScale());

                // ������� ������� �� ����
                Destroy(hit.collider.gameObject);

                // ������������� ����, ��� ����� ������ �������
                isHoldingItem = true;
            }
        }
        else
        {
            Debug.Log("�� ������ �������");
        }
    }

    private void DisplayItemInHand(ItemSO item, Vector3 originalScale)
    {
        // ������� ������� ������� �� ���, ���� �� ����
        if (currentHeldItem != null)
        {
            Destroy(currentHeldItem);
        }

        // ������ ����� ������ �������� � �����
        if (item.prefab != null)
        {
            currentHeldItem = Instantiate(item.prefab, handTransform);
            currentHeldItem.transform.localPosition = Vector3.zero; // ������� � �����
            currentHeldItem.transform.localRotation = Quaternion.identity; // ������� � �����
            currentHeldItem.transform.localScale = originalScale; // ��������� ������������ ������

            // ��������� MeshCollider, ���� ��� ���
            MeshCollider meshCollider = currentHeldItem.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = currentHeldItem.AddComponent<MeshCollider>();
                meshCollider.convex = true; // �������� convex ��� ������ � Rigidbody
            }
        }
    }

    private void DropItem()
    {
        // ���� � ����� ���� �������, ����������� ���
        if (currentHeldItem != null)
        {
            // ������� ������� �� ���
            Destroy(currentHeldItem);

            // ������ ������� � ����
            ItemSO item = inventory.GetCurrentItem();
            if (item != null && item.prefab != null)
            {
                // ������� ������
                Vector3 spawnPosition = handTransform.position + handTransform.forward * 2;

                // ������ ������ �������� � ����
                GameObject droppedItem = Instantiate(item.prefab, spawnPosition, Quaternion.identity);
                if (droppedItem == null)
                {
                    return;
                }

                // ��������������� ������������ ������
                droppedItem.transform.localScale = heldItemOriginalScale;

                // ��������� MeshCollider, ���� ��� ���
                MeshCollider meshCollider = droppedItem.GetComponent<MeshCollider>();
                if (meshCollider == null)
                {
                    meshCollider = droppedItem.AddComponent<MeshCollider>();
                    meshCollider.convex = true; // �������� convex ��� ������ � Rigidbody
                }

                // ��������� Rigidbody, ���� ��� ���
                Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = droppedItem.AddComponent<Rigidbody>();
                }

                // ����������� Rigidbody
                rb.mass = 1f; // ������� �����, ���� �����
                rb.interpolation = RigidbodyInterpolation.Interpolate; // ��� �������� ��������
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // ��� �������������� ������������

                // ������� ������� �� ���������
                inventory.RemoveItem(item);
            }

            // ���������� ����, ��� ����� ������ �������
            isHoldingItem = false;
        }
    }
}