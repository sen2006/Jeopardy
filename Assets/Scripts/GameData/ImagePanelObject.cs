using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ImagePanelObject : PanelObject {
    Texture2D texture = new Texture2D(2,2);

    public override GameObject LoadToScene(GameObject parent) {
        GameObject obj = new GameObject("ClipboardImage", typeof(RectTransform), typeof(RawImage));

        obj.transform.SetParent(parent.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
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
        rect.anchoredPosition = Vector2.zero;
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(texture.width, texture.height);
        rect.localScale = new Vector2(xScale, yScale);

        RawImage image = obj.GetComponent<RawImage>();
        image.texture = texture;
    }

    private GameObject createObject() {
        GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Panel.prefab", typeof(GameObject));
        return Object.Instantiate(obj, Vector3.zero, Quaternion.identity);
    }

    public override void Deserialize(Packet pPacket) {
       Texture2D tex = new Texture2D(2,2);
        
        x = pPacket.ReadFloat();
        y = pPacket.ReadFloat();
        xScale = pPacket.ReadFloat();
        yScale = pPacket.ReadFloat();

        byte[] bytes = pPacket.readByteArray();
        tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        texture = tex;
    }

    public override void Serialize(Packet pPacket) {
        pPacket.Write(x);
        pPacket.Write(y);
        pPacket.Write(xScale);
        pPacket.Write(yScale);
        pPacket.WriteByteArray(texture.EncodeToPNG());
    }

    public void SetTexture(Texture2D texture) {
        this.texture = texture;
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
