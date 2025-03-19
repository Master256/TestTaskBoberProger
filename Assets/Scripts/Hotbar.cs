using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private Image[] slots; // Массив ячеек хот-бара
    [SerializeField] private Sprite defaultIcon; // Иконка по умолчанию
    [SerializeField] private PickupItem pickupItem; // Ссылка на скрипт PickupItem
    private int selectedSlot = 0; // Текущая выбранная ячейка

    private void Start()
    {
        UpdateHotbar();
    }

    private void Update()
    {
        HandleScrollWheel();
    }

    private void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            selectedSlot -= (int)Mathf.Sign(scroll);
            if (selectedSlot < 0) selectedSlot = slots.Length - 1;
            if (selectedSlot >= slots.Length) selectedSlot = 0;
            UpdateHotbar();

            pickupItem.SetActiveObject(); // Обновляем предмет в руках
        }
    }

    public void UpdateHotbar()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].sprite == defaultIcon)
            {
                if (i == selectedSlot)
                {
                    slots[i].color = Color.white;
                }
                else
                {
                    slots[i].color = Color.gray;
                }
            }
        }
    }

    public void SetItemIcon(int slotIndex, Sprite icon)
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex].sprite = icon;
        }
    }

    public void ClearItemIcon(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex].sprite = defaultIcon;
        }
    }

    public int GetLengthSlots()
    {
        return slots.Length;
    }

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }
}