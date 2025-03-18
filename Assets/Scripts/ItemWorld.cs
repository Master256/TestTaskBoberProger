using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    [SerializeField] private ItemSO item; // ������ �� ScriptableObject ��������
    private Vector3 originalScale; // ������������ ������ �������

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