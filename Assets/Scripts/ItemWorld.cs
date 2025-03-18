using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private new Rigidbody rigidbody;
    private Vector3 originalScale;

    private void Awake()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

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

    public void SetItem(ItemSO newItem)
    {
        item = newItem;
    }
}