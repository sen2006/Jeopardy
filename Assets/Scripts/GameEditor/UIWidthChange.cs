using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIWidthChange : MonoBehaviour, IScrollHandler {
	List<RectTransform> transforms = new();
	public float scaleSpeed = 5;

	void Start() {
		transforms.Add(GetComponent<RectTransform>());
		foreach (Transform child in transform) {
			transforms.Add((RectTransform)child);
		}
	}

	public void OnScroll(PointerEventData eventData) {
        if (!Keyboard.current.leftShiftKey.isPressed) return;
        float scroll = eventData.scrollDelta.y;

		foreach (RectTransform tf in transforms) {
			float scale = tf.sizeDelta.x;
			scale += scroll * scaleSpeed;

			float clamped = Mathf.Clamp(scale, 50, 19200);
			tf.sizeDelta = new Vector2(clamped, tf.sizeDelta.y);
		}
	}
}