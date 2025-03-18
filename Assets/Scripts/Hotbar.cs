using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private Image[] slots; // Массив ячеек хот-бара
    [SerializeField] private Sprite defaultIcon; // Иконка по умолчанию
    private int selectedSlot = 0; // Текущая выбранная ячейка

    void Start()
    {
        UpdateHotbar();
    }

    void Update()
    {
        HandleScrollWheel();
    }

    void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            selectedSlot -= (int)Mathf.Sign(scroll);
            if (selectedSlot < 0) selectedSlot = slots.Length - 1;
            if (selectedSlot >= slots.Length) selectedSlot = 0;
            UpdateHotbar();
        }
    }

    public void UpdateHotbar()
    {
        for (int i = 0; i < slots.Length; i++)
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
}