using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 3f; // Дистанция подбора предметов
    [SerializeField] private LayerMask itemLayer; // Слой для предметов
    [SerializeField] private Transform handTransform; // Точка, где будет отображаться предмет в руках
    [SerializeField] private GameInput gameInput; // Ссылка на GameInput

    private Inventory inventory; // Ссылка на инвентарь
    private GameObject currentHeldItem; // Текущий предмет в руках
    private Vector3 heldItemOriginalScale; // Оригинальный размер предмета в руках
    private Vector3 heldItemColliderSize; // Оригинальный размер коллайдера
    private bool isHoldingItem = false; // Флаг, указывающий, держит ли игрок предмет

    private void Awake()
    {
        inventory = GetComponent<Inventory>(); // Получаем компонент Inventory
        if (inventory == null)
        {
            Debug.LogError("Inventory component not found on the player!");
        }
    }

    private void Update()
    {
        // Проверяем, нажата ли клавиша взаимодействия
        if (gameInput.IsInteractPressed())
        {
            if (isHoldingItem)
            {
                // Если игрок держит предмет, отпускаем его
                DropItem();
            }
            else
            {
                // Если игрок не держит предмет, пытаемся подобрать
                TryPickupItem();
            }
        }
    }

    private void TryPickupItem()
    {
        // Бросаем луч для обнаружения предметов
        RaycastHit hit;
        bool isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupDistance, itemLayer);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * pickupDistance, Color.red, 1f); // Рисуем луч в сцене

        if (isHit)
        {
            ItemWorld itemWorld = hit.collider.GetComponent<ItemWorld>();
            if (itemWorld != null)
            {
                Debug.Log($"Найден предмет: {itemWorld.name}");
                // Добавляем предмет в инвентарь
                inventory.AddItem(itemWorld.GetItem());

                // Отображаем предмет в руках
                DisplayItemInHand(itemWorld.GetItem(), itemWorld.GetOriginalScale());

                // Убираем предмет из мира
                Destroy(hit.collider.gameObject);

                // Устанавливаем флаг, что игрок держит предмет
                isHoldingItem = true;
            }
        }
        else
        {
            Debug.Log("Не найден предмет");
        }
    }

    private void DisplayItemInHand(ItemSO item, Vector3 originalScale)
    {
        // Удаляем текущий предмет из рук, если он есть
        if (currentHeldItem != null)
        {
            Destroy(currentHeldItem);
        }

        // Создаём новый объект предмета в руках
        if (item.prefab != null)
        {
            currentHeldItem = Instantiate(item.prefab, handTransform);
            currentHeldItem.transform.localPosition = Vector3.zero; // Позиция в руках
            currentHeldItem.transform.localRotation = Quaternion.identity; // Поворот в руках
            currentHeldItem.transform.localScale = originalScale; // Сохраняем оригинальный размер

            // Добавляем MeshCollider, если его нет
            MeshCollider meshCollider = currentHeldItem.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = currentHeldItem.AddComponent<MeshCollider>();
                meshCollider.convex = true; // Включаем convex для работы с Rigidbody
            }
        }
    }

    private void DropItem()
    {
        // Если в руках есть предмет, выбрасываем его
        if (currentHeldItem != null)
        {
            // Убираем предмет из рук
            Destroy(currentHeldItem);

            // Создаём предмет в мире
            ItemSO item = inventory.GetCurrentItem();
            if (item != null && item.prefab != null)
            {
                // Позиция спавна
                Vector3 spawnPosition = handTransform.position + handTransform.forward * 2;

                // Создаём объект предмета в мире
                GameObject droppedItem = Instantiate(item.prefab, spawnPosition, Quaternion.identity);
                if (droppedItem == null)
                {
                    return;
                }

                // Восстанавливаем оригинальный размер
                droppedItem.transform.localScale = heldItemOriginalScale;

                // Добавляем MeshCollider, если его нет
                MeshCollider meshCollider = droppedItem.GetComponent<MeshCollider>();
                if (meshCollider == null)
                {
                    meshCollider = droppedItem.AddComponent<MeshCollider>();
                    meshCollider.convex = true; // Включаем convex для работы с Rigidbody
                }

                // Добавляем Rigidbody, если его нет
                Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = droppedItem.AddComponent<Rigidbody>();
                }

                // Настраиваем Rigidbody
                rb.mass = 1f; // Настрой массу, если нужно
                rb.interpolation = RigidbodyInterpolation.Interpolate; // Для плавного движения
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Для предотвращения проваливания

                // Убираем предмет из инвентаря
                inventory.RemoveItem(item);
            }

            // Сбрасываем флаг, что игрок держит предмет
            isHoldingItem = false;
        }
    }
}