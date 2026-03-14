using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class URLPanelObject : PanelObject {
    string URL;

    public override GameObject LoadToScene(GameObject parent) {
        GameObject obj = new GameObject("URL", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(Button));
        Button button = obj.GetComponent<Button>();
        button.onClick.AddListener(OpenURL);

        obj.transform.SetParent(parent.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(500, 125);
        rect.localScale = new Vector2(xScale, yScale);

        TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();

        textMesh.text = URL;
        return obj;
    }

    private void OpenURL() {
        Application.OpenURL(URL);
    }

    public override void LoadToEditorScene(GameObject parent) {
        GameObject obj = new GameObject("URLField", typeof(RectTransform), typeof(TMP_InputField), typeof(EditorURLPanelObject), typeof(DragableUI));
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


        textInput.text = URL;
    }


    public override void Deserialize(Packet pPacket) {
       Texture2D tex = new Texture2D(2,2);
        
        x = pPacket.ReadFloat();
        y = pPacket.ReadFloat();
        xScale = pPacket.ReadFloat();
        yScale = pPacket.ReadFloat();
        URL = pPacket.ReadString();
    }

    public override void Serialize(Packet pPacket) {
        pPacket.Write(x);
        pPacket.Write(y);
        pPacket.Write(xScale);
        pPacket.Write(yScale);
        pPacket.Write(URL);
    }

    public void SetText(string text) {
        this.URL = text;
    }
}

[RequireComponent(typeof(RawImage))]
public class EditorURLPanelObject : EditorPanelObject<URLPanelObject> {
    public override URLPanelObject getData() {
        URLPanelObject toReturn = new URLPanelObject();
        Vector3 pos = gameObject.transform.localPosition;
        Vector3 scale = gameObject.transform.localScale;
        toReturn.SetXY(pos.x, pos.y);
        toReturn.SetScale(scale.x, scale.y);
        toReturn.SetText(gameObject.GetComponent<TMP_InputField>().text);
        return toReturn;
    }

}
