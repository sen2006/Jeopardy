using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler {
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;

    [Header("Scaling")]
    public float scaleSpeed = 0.0025f;
    public float minScale = 0.05f;
    public float maxScale = 4f;
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );

        // Adjust offset by current scale
        //offset *= rectTransform.localScale.x;

        // Optional: bring to front
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint)) {
            // Apply offset scaled by current scale
            rectTransform.localPosition = localPoint - offset * rectTransform.localScale.x;
        }
    }

    public void OnScroll(PointerEventData eventData) {
        float scroll = eventData.scrollDelta.y;

        Vector3 scale = rectTransform.localScale;
        scale += Vector3.one * scroll * scaleSpeed;

        float clamped = Mathf.Clamp(scale.x, minScale, maxScale);
        rectTransform.localScale = new Vector3(clamped, clamped, clamped);
    }
}