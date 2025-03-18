using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSO> items = new List<ItemSO>(); // Список предметов в инвентаре

    public void AddItem(ItemSO item)
    {
        if (items.Count < 5) // Ограничение на количество предметов в инвентаре
        {
            items.Add(item);
            UpdateUI(); // Обновляем интерфейс инвентаря
        }
    }

    public void RemoveItem(ItemSO item)
    {
        items.Remove(item);
        UpdateUI(); // Обновляем интерфейс инвентаря
    }

    public ItemSO GetCurrentItem()
    {
        if (items.Count > 0)
        {
            return items[items.Count - 1]; // Возвращаем последний добавленный предмет
        }
        return null;
    }

    private void UpdateUI()
    {
        // Логика обновления UI инвентаря
        // Например, отображение иконок предметов
    }
}