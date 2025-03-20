using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxDistance = 5f; // Максимальная дистанция взаимодействия
    [SerializeField] private LayerMask interactableLayer; // Слой для подсвечиваемых объектов

    private HighlightableObject lastHighlighted; // Последний подсвеченный объект

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            // Если нашли новый объект
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