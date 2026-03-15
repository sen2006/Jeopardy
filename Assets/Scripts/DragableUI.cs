using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler, IPointerEnterHandler, IPointerExitHandler {
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;
    private bool isHovered = false;

    [Header("Scaling")]
    public float scaleSpeed = 0.0025f;
    public float minScale = 0.05f;
    public float maxScale = 4f;
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update() {
        // Delete object
        if (isHovered && Keyboard.current.deleteKey.wasPressedThisFrame) {
            Destroy(gameObject);
        }

        // Copy object (CTRL + C)
        if (isHovered &&
            Keyboard.current.ctrlKey.isPressed &&
            Keyboard.current.cKey.wasPressedThisFrame) {

            GameObject copy = Instantiate(gameObject, transform.parent);

            RectTransform copyRect = copy.GetComponent<RectTransform>();

            // Offset the copy slightly
            copyRect.localPosition = rectTransform.localPosition + new Vector3(20, -20, 0);

            copyRect.localScale = rectTransform.localScale;
        }
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
        if (Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.leftAltKey.isPressed ||
            Keyboard.current.leftCtrlKey.isPressed) return;
        float scroll = eventData.scrollDelta.y;

        Vector3 scale = rectTransform.localScale;
        scale += Vector3.one * scroll * scaleSpeed;

        float clamped = Mathf.Clamp(scale.x, minScale, maxScale);
        rectTransform.localScale = new Vector3(clamped, clamped, clamped);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovered = false;
    }
}