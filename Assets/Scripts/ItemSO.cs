using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public string itemName; // �������� ��������
    public Sprite itemIcon; // ������ ��� ���������
    public GameObject prefab; // ������ ��������
    public ItemType itemType; // ��� ��������

    public enum ItemType
    {
        Key,
        Coin,
        Box
    }
}