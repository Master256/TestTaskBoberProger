using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemSO[] items; // Массив для хранения предметов по слотам
    [SerializeField] private Hotbar hotbar;

    private void Start()
    {
        // Инициализируем массив items с размером, равным количеству слотов в хот-баре
        items = new ItemSO[hotbar.GetLengthSlots()];
    }

    // Добавляем предмет в инвентарь
    public void AddItem(ItemSO item)
    {
        int slotIndex = hotbar.GetSelectedSlot();

        // Если слот пустой, добавляем предмет
        if (items[slotIndex] == null)
        {
            items[slotIndex] = item;
            hotbar.SetItemIcon(slotIndex, item.itemIcon);
            UpdateUI();
        }
    }

    // Убираем предмет из инвентаря
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Length && items[slotIndex] != null)
        {
            items[slotIndex] = null;
            hotbar.ClearItemIcon(slotIndex);
            UpdateUI();
        }
    }

    // Возвращаем текущий выбранный предмет
    public ItemSO GetCurrentItem()
    {
        int slotIndex = hotbar.GetSelectedSlot();
        if (slotIndex >= 0 && slotIndex < items.Length)
        {
            return items[slotIndex];
        }
        return null;
    }

    // Проверяем, есть ли предмет в конкретном слоте
    public bool HasItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Length)
        {
            return items[slotIndex] != null;
        }
        return false;
    }

    // Обновляем интерфейс инвентаря
    private void UpdateUI()
    {
        // Здесь можно добавить логику обновления UI, если нужно
    }
}
