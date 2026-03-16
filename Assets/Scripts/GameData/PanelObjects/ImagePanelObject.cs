using System;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanelObject : PanelObject {
    Texture2D texture = new Texture2D(2,2);

    public override GameObject LoadToScene(GameObject parent) {
        GameObject obj = new GameObject("ClipboardImage", typeof(RectTransform), typeof(RawImage));

        obj.transform.SetParent(parent.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(texture.width, texture.height);
        rect.localScale = new Vector2(xScale, yScale);

        RawImage image = obj.GetComponent<RawImage>();

        image.texture = texture;
        return obj;
    }

    public override void LoadToEditorScene(GameObject parent) {
        GameObject obj = new GameObject("ClipboardImage", typeof(RectTransform), typeof(RawImage), typeof(EditorImagePanelObject), typeof(DragableUI));

        obj.transform.SetParent(parent.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(texture.width, texture.height);
        rect.localScale = new Vector2(xScale, yScale);

        RawImage image = obj.GetComponent<RawImage>();
        image.texture = texture;
    }

    public void SetTexture(Texture2D texture) {
        this.texture = texture;
    }

    public override PanelObjectSaveData Save() {
        PanelObjectSaveData save = new();
        save.aditionalStringData = new() {
            TextureToBase64(texture)
        };
        save.x = x;
        save.y = y;
        save.xScale = xScale;
        save.yScale = yScale;
        return save;
    }

    public override void Load(PanelObjectSaveData saveData) {
        texture = Base64ToTexture(saveData.aditionalStringData[0]);
        x = saveData.x;
        y = saveData.y;
        xScale = saveData.xScale;
        yScale = saveData.yScale;
    }

    public static string TextureToBase64(Texture2D texture) {
        byte[] pngBytes = texture.EncodeToJPG();
        return Convert.ToBase64String(pngBytes);
    }

    public static Texture2D Base64ToTexture(string base64) {
        byte[] bytes = Convert.FromBase64String(base64);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        return texture;
    }
}

[RequireComponent(typeof(RawImage))]
public class EditorImagePanelObject : EditorPanelObject<ImagePanelObject> {
    public override ImagePanelObject getData() {
        ImagePanelObject toReturn = new ImagePanelObject();
        Vector3 pos = gameObject.transform.localPosition;
        Vector3 scale = gameObject.transform.localScale;
        toReturn.SetXY(pos.x, pos.y);
        toReturn.SetScale(scale.x, scale.y);
        toReturn.SetTexture((Texture2D)gameObject.GetComponent<RawImage>().texture);
        return toReturn;
    }

}
