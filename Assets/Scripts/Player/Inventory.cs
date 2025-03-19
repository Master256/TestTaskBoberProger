using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemSO[] items; // ������ ��� �������� ��������� �� ������
    [SerializeField] private Hotbar hotbar;

    private void Start()
    {
        // �������������� ������ items � ��������, ������ ���������� ������ � ���-����
        items = new ItemSO[hotbar.GetLengthSlots()];
    }

    // ��������� ������� � ���������
    public void AddItem(ItemSO item)
    {
        int slotIndex = hotbar.GetSelectedSlot();

        // ���� ���� ������, ��������� �������
        if (items[slotIndex] == null)
        {
            items[slotIndex] = item;
            hotbar.SetItemIcon(slotIndex, item.itemIcon);
            UpdateUI();
        }
    }

    // ������� ������� �� ���������
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Length && items[slotIndex] != null)
        {
            items[slotIndex] = null;
            hotbar.ClearItemIcon(slotIndex);
            UpdateUI();
        }
    }

    // ���������� ������� ��������� �������
    public ItemSO GetCurrentItem()
    {
        int slotIndex = hotbar.GetSelectedSlot();
        if (slotIndex >= 0 && slotIndex < items.Length)
        {
            return items[slotIndex];
        }
        return null;
    }

    // ���������, ���� �� ������� � ���������� �����
    public bool HasItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Length)
        {
            return items[slotIndex] != null;
        }
        return false;
    }

    // ��������� ��������� ���������
    private void UpdateUI()
    {
        // ����� ����� �������� ������ ���������� UI, ���� �����
    }
}
