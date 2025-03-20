using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxDistance = 5f; // ������������ ��������� ��������������
    [SerializeField] private LayerMask interactableLayer; // ���� ��� �������������� ��������

    private HighlightableObject lastHighlighted; // ��������� ������������ ������

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            // ���� ����� ����� ������
            if (hit.collider.TryGetComponent(out HighlightableObject highlightable))
            {
                if (lastHighlighted != highlightable)
                {
                    ResetPreviousHighlight();
                    lastHighlighted = highlightable;
                    highlightable.ShowHighlight();
                }
            }
            else
            {
                ResetPreviousHighlight();
            }
        }
        else
        {
            ResetPreviousHighlight();
        }
    }

    private void ResetPreviousHighlight()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.HideHighlight();
            lastHighlighted = null;
        }
    }
}