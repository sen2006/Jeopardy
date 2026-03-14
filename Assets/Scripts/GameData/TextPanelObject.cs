using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextPanelObject : PanelObject {
    string text;

    public override GameObject LoadToScene(GameObject parent) {
        GameObject obj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));

        obj.transform.SetParent(parent.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(500, 125);
        rect.localScale = new Vector2(xScale, yScale);

        TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();

        textMesh.text = text;
        return obj;
    }

    public override void LoadToEditorScene(GameObject parent) {
        GameObject obj = new GameObject("TextField", typeof(RectTransform), typeof(TMP_InputField), typeof(EditorTextPanelObject), typeof(DragableUI));
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));

        obj.transform.SetParent(parent.transform, false);
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


    public override void Deserialize(Packet pPacket) {
       Texture2D tex = new Texture2D(2,2);
        
        x = pPacket.ReadFloat();
        y = pPacket.ReadFloat();
        xScale = pPacket.ReadFloat();
        yScale = pPacket.ReadFloat();
        text = pPacket.ReadString();
    }

    public override void Serialize(Packet pPacket) {
        pPacket.Write(x);
        pPacket.Write(y);
        pPacket.Write(xScale);
        pPacket.Write(yScale);
        pPacket.Write(text);
    }

    public void SetText(string text) {
        this.text = text;
    }
}

[RequireComponent(typeof(RawImage))]
public class EditorTextPanelObject : EditorPanelObject<TextPanelObject> {
    public override TextPanelObject getData() {
        TextPanelObject toReturn = new TextPanelObject();
        Vector3 pos = gameObject.transform.localPosition;
        Vector3 scale = gameObject.transform.localScale;
        toReturn.SetXY(pos.x, pos.y);
        toReturn.SetScale(scale.x, scale.y);
        toReturn.SetText(gameObject.GetComponent<TMP_InputField>().text);
        return toReturn;
    }

}
