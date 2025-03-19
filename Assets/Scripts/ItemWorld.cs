using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private new Rigidbody rigidbody;
    private Vector3 originalScale;
    private Vector3 colliderSize;
    private Vector3 colliderCenter;

    private void Awake()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        originalScale = transform.localScale;

        // ѕолучаем данные коллайдера, если он есть
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            colliderSize = collider.size;
            colliderCenter = collider.center;
        }
        else
        {
            colliderSize = Vector3.one;
            colliderCenter = Vector3.zero;
        }
    }

    public ItemSO GetItem()
    {
        return item;
    }

    public Vector3 GetOriginalScale()
    {
        return originalScale;
    }

    public Vector3 GetColliderSize()
    {
        return colliderSize;
    }

    public Vector3 GetColliderCenter()
    {
        return colliderCenter;
    }

    public void SetItem(ItemSO newItem)
    {
        item = newItem;
    }
}