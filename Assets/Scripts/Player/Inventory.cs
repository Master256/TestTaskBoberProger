using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSO> items = new List<ItemSO>(); // ������ ��������� � ���������

    public void AddItem(ItemSO item)
    {
        if (items.Count < 5) // ����������� �� ���������� ��������� � ���������
        {
            items.Add(item);
            UpdateUI(); // ��������� ��������� ���������
        }
    }

    public void RemoveItem(ItemSO item)
    {
        items.Remove(item);
        UpdateUI(); // ��������� ��������� ���������
    }

    public ItemSO GetCurrentItem()
    {
        if (items.Count > 0)
        {
            return items[items.Count - 1]; // ���������� ��������� ����������� �������
        }
        return null;
    }

    private void UpdateUI()
    {
        // ������ ���������� UI ���������
        // ��������, ����������� ������ ���������
    }
}