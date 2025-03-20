using UnityEngine;

public class HighlightableObject : MonoBehaviour
{
    [Header("Highlight Settings")]
    public Material highlightMaterial;
    public Renderer[] targetRenderers;
    [SerializeField] private float highlightScale = 1.02f;

    private GameObject[] highlightObjects;

    public void SetRenderers(Renderer[] renderers)
    {
        targetRenderers = RemoveNullRenderers(renderers);
        Initialize();
    }

    private void Start()
    {
        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            targetRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
        }

        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            return;
        }

        Initialize();
    }

    private void Initialize()
    {
        targetRenderers = RemoveNullRenderers(targetRenderers);

        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            return;
        }

        CreateHighlightObjects();
    }

    private Renderer[] RemoveNullRenderers(Renderer[] renderers)
    {
        return System.Array.FindAll(renderers, r => r != null);
    }

    private void CreateHighlightObjects()
    {
        highlightObjects = new GameObject[targetRenderers.Length];
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] != null)
            {
                highlightObjects[i] = CreateHighlightObject(targetRenderers[i]);
            }
        }
    }

    private GameObject CreateHighlightObject(Renderer targetRenderer)
    {
        GameObject highlightObject = new GameObject("Highlight");
        highlightObject.transform.SetParent(targetRenderer.transform);
        highlightObject.transform.localPosition = Vector3.zero;
        highlightObject.transform.localRotation = Quaternion.identity;
        highlightObject.transform.localScale = Vector3.one * highlightScale;

        MeshFilter meshFilter = highlightObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = highlightObject.AddComponent<MeshRenderer>();

        if (targetRenderer is MeshRenderer)
        {
            meshFilter.mesh = targetRenderer.GetComponent<MeshFilter>().mesh;
        }
        else if (targetRenderer is SkinnedMeshRenderer)
        {
            meshFilter.mesh = ((SkinnedMeshRenderer)targetRenderer).sharedMesh;
        }

        meshRenderer.material = highlightMaterial;
        highlightObject.SetActive(false);

        return highlightObject;
    }

    public void ShowHighlight()
    {
        if (highlightObjects == null) return;

        foreach (GameObject highlightObject in highlightObjects)
        {
            if (highlightObject != null)
            {
                highlightObject.SetActive(true);
            }
        }
    }

    public void HideHighlight()
    {
        if (highlightObjects == null) return;

        foreach (GameObject highlightObject in highlightObjects)
        {
            if (highlightObject != null)
            {
                highlightObject.SetActive(false);
            }
        }
    }
}
