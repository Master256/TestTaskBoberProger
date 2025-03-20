using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private new Rigidbody rigidbody;
    private Vector3 originalScale;
    private Vector3 colliderSize;
    private Vector3 colliderCenter;
    private HighlightableObject highlightableObject;
    private Renderer[] originalRenderers;

    private void Awake()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        originalScale = transform.localScale;

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

        highlightableObject = GetComponent<HighlightableObject>();

        if (highlightableObject != null && highlightableObject.targetRenderers != null && highlightableObject.targetRenderers.Length > 0)
        {
            originalRenderers = highlightableObject.targetRenderers;
        }
        else
        {
            originalRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
        }

        if (highlightableObject != null)
        {
            highlightableObject.SetRenderers(originalRenderers);
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

    public Material GetOriginalMaterial()
    {
        if (highlightableObject != null)
        {
            return highlightableObject.highlightMaterial;
        }
        return null;
    }

    public Renderer[] GetOriginalRenderers()
    {
        return originalRenderers;
    }

    public void SetItem(ItemSO newItem)
    {
        item = newItem;
    }
}