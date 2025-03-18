using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> items = new List<ItemSO>();
    private int selectedIndex = -1; // ������ �������� ���������� �������� (-1 ��������, ��� ������ �� �������)

    // ��������� ������� � ���������
    public void AddItem(ItemSO item)
    {
        if (items.Count < 5)
        {
            items.Add(item);
            if (selectedIndex == -1)
            {
                selectedIndex = items.Count - 1;
            }
            UpdateUI();
        }
    }

    // ������� ������� �� ���������
    public void RemoveItem(ItemSO item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            if (selectedIndex >= items.Count)
            {
                selectedIndex = items.Count - 1;
            }
            UpdateUI();
        }
    }

    // ���������� ������� ��������� �������
    public ItemSO GetCurrentItem()
    {
        if (selectedIndex >= 0 && selectedIndex < items.Count)
        {
            return items[selectedIndex];
        }
        return null;
    }

    // ������������� �� ��������� �������
    public void NextItem()
    {
        if (items.Count > 0)
        {
            selectedIndex = (selectedIndex + 1) % items.Count;
            UpdateUI();
        }
    }

    // ������������� �� ���������� �������
    public void PreviousItem()
    {
        if (items.Count > 0)
        {
            selectedIndex = (selectedIndex - 1 + items.Count) % items.Count;
            UpdateUI();
        }
    }

    // ��������� ��������� ���������
    private void UpdateUI()
    {

    }
}