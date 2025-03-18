using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public string itemName; // Название предмета
    public Sprite itemIcon; // Иконка для инвентаря
    public GameObject prefab; // Префаб предмета
    public ItemType itemType; // Тип предмета

    public enum ItemType
    {
        Key,
        Coin,
        Box
    }
}