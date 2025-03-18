using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    [SerializeField] private ItemSO item; // —сылка на ScriptableObject предмета
    private Vector3 originalScale; // ќригинальный размер объекта

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public ItemSO GetItem()
    {
        return item;
    }

    public Vector3 GetOriginalScale()
    {
        return originalScale;
    }
}