using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPanelObject : PanelObject {
    string text;
    float objWidth;

    public override GameObject LoadToScene(GameObject parent) {
        GameObject obj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));

        obj.transform.SetParent(parent.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(objWidth, 125);
        rect.localScale = new Vector2(xScale, yScale);

        TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.fontSize = 100;
        return obj;
    }

    public override void LoadToEditorScene(GameObject parent) {
        GameObject obj = new GameObject("TextField", typeof(RectTransform), typeof(TMP_InputField), typeof(EditorTextPanelObject), typeof(DragableUI), typeof(UIWidthChange));
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));

        obj.transform.SetParent(parent.transform, false);
        textObj.transform.SetParent(obj.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(objWidth, 125);
        rect.localScale = new Vector2(xScale, yScale);

        RectTransform rectText = textObj.GetComponent<RectTransform>();
        rectText.anchoredPosition = Vector2.zero;
        rectText.sizeDelta = new Vector2(objWidth, 125);

        textObj.GetComponent<TextMeshProUGUI>().fontSize = 100;

        TMP_InputField textInput = obj.GetComponent<TMP_InputField>();
        textInput.lineType = TMP_InputField.LineType.MultiLineNewline;
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
        textInput.scrollSensitivity = 0;
        textInput.GetComponent<RawImage>().color = new Color(0, 0, 0, 0.2f);


        textInput.text = text;
    }

    public void SetText(string text) {
        this.text = text;
    }

    public override PanelObjectSaveData Save() {
        PanelObjectSaveData save = new();

        save.aditionalStringData = new() {
            text
        };

        save.aditionalFloatData = new() {
            objWidth
        };
        save.x = x;
        save.y = y;
        save.xScale = xScale;
        save.yScale = yScale;
        return save;
    }

    public override void Load(PanelObjectSaveData saveData) {
        text = saveData.aditionalStringData[0];
        objWidth = saveData.aditionalFloatData[0];
        x = saveData.x;
        y = saveData.y;
        xScale = saveData.xScale;
        yScale = saveData.yScale;
    }

    public void SetWidth(float w) {
        objWidth = w;
    }
}

[RequireComponent(typeof(RawImage))]
public class EditorTextPanelObject : EditorPanelObject<TextPanelObject> {
    public override TextPanelObject getData() {
        TextPanelObject toReturn = new TextPanelObject();
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 pos = rectTransform.localPosition;
        Vector3 scale = rectTransform.localScale;
        toReturn.SetXY(pos.x, pos.y);
        toReturn.SetScale(scale.x, scale.y);
        toReturn.SetWidth(rectTransform.sizeDelta.x);
        toReturn.SetText(gameObject.GetComponent<TMP_InputField>().text);
        return toReturn;
    }
}
