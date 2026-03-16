using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorURLButton : MonoBehaviour {

    [SerializeField] Button button;
    [SerializeField] GameObject canvas;

    private void Start() {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        CreateUI("www.google.com");
    }

    private void CreateUI(string text) {
        GameObject obj = new GameObject("URLField", typeof(RectTransform), typeof(TMP_InputField), typeof(EditorURLPanelObject), typeof(DragableUI));
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));

        obj.transform.SetParent(canvas.transform, false);
        textObj.transform.SetParent(obj.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(500, 125);

        RectTransform rectText = textObj.GetComponent<RectTransform>();
        rectText.anchoredPosition = Vector2.zero;
        rectText.sizeDelta = new Vector2(500, 125);

        textObj.GetComponent<TextMeshProUGUI>().fontSize = 100;

        TMP_InputField textInput = obj.GetComponent<TMP_InputField>();
        textInput.transition = Selectable.Transition.ColorTint;
        textInput.targetGraphic = textInput.GetComponent<RawImage>();

        ColorBlock colors = new ColorBlock();
        colors.normalColor = new Color(0, 0, 0, 0);
        colors.highlightedColor = new Color(0, 0, 0, 0.5f);
        colors.pressedColor = new Color(0, 0, 0, 0.7f);
        colors.selectedColor = colors.pressedColor;
        colors.disabledColor = new Color(0, 0, 0, 0);
        colors.colorMultiplier = 1;

        textInput.colors = colors;


        textInput.textComponent = textObj.GetComponent<TextMeshProUGUI>();
        textInput.textViewport = rect;
        textInput.GetComponent<RawImage>().color = new Color(0, 0, 0, 0.2f);


        textInput.text = text;
    }
}
