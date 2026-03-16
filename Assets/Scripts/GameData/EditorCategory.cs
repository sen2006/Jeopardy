using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EditorCategory : MonoBehaviour {
	CategoryData linkedCategory;

    private void Awake() {
		GameObject child = new GameObject("TitleInput", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(TMP_InputField));
		child.transform.SetParent(transform);
		child.GetComponent<Image>().color = new Color(0,0,0,0);
		RectTransform rect = (child.transform as RectTransform);
		TMP_InputField inputField = child.GetComponent<TMP_InputField>();
		inputField.transition = Selectable.Transition.None;
		inputField.textComponent = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        inputField.text = "title";

        rect.sizeDelta = new Vector2(300, 150);
		rect.anchorMin = new Vector2(.5f, 1);
		rect.anchorMax = new Vector2(.5f, 1);
		rect.anchoredPosition = new Vector2(0, -75);

        inputField.textViewport = rect;

		inputField.onValueChanged.AddListener(TextUpdated);
    }

	private void TextUpdated(string newText) {
		linkedCategory.SetName(newText);
	}

    public void SetCategory(CategoryData category) {
		linkedCategory = category;
	}

	public void SetName(string name) {
		linkedCategory.SetName(name);
	}

    internal string GetName() {
        return name;
    }
}