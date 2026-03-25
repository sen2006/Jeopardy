using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoURLPanelObject : PanelObject {
    string url;

    public override GameObject LoadToScene(GameObject parent) {
        GameObject obj = new GameObject("VideoURL",
            typeof(RectTransform),
            typeof(RawImage),
            typeof(VideoPlayer)
        );

        obj.transform.SetParent(parent.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(500, 300);
        rect.localScale = new Vector2(xScale, yScale);
        RawImage image = obj.GetComponent<RawImage>();
        VideoPlayer player = obj.GetComponent<VideoPlayer>();
        RenderTexture rt = new RenderTexture(512, 512, 0);
        image.texture = rt;
        player.renderMode = VideoRenderMode.RenderTexture;
        player.targetTexture = rt;
        player.url = url;
        player.playOnAwake = false;
        player.isLooping = true;
        return obj;
    }

    public override void LoadToEditorScene(GameObject parent) {
        GameObject obj = new GameObject("VideoURL",
            typeof(RectTransform),
            typeof(RawImage),
            typeof(VideoPlayer),
            typeof(DragableUI)
        );

        obj.transform.SetParent(parent.transform, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(500, 300);
        rect.localScale = new Vector2(xScale, yScale);
        RawImage image = obj.GetComponent<RawImage>();
        VideoPlayer player = obj.GetComponent<VideoPlayer>();
        RenderTexture rt = new RenderTexture(512, 512, 0);
        image.texture = rt;
        player.renderMode = VideoRenderMode.RenderTexture;
        player.targetTexture = rt;
        player.url = url;
        player.playOnAwake = false;
        player.isLooping = true;
    }

    public void SetURL(string url) {
        this.url = url;
    }

    public override PanelObjectSaveData Save() {
        return new PanelObjectSaveData {
            aditionalStringData = new() { url },
            x = x,
            y = y,
            xScale = xScale,
            yScale = yScale
        };
    }

    public override void Load(PanelObjectSaveData saveData) {
        url = saveData.aditionalStringData[0];
        x = saveData.x;
        y = saveData.y;
        xScale = saveData.xScale;
        yScale = saveData.yScale;
    }
}



[RequireComponent(typeof(VideoPlayer))]
public class EditorVideoURLPanelObject : EditorPanelObject<VideoURLPanelObject> {
    public override VideoURLPanelObject getData() {
        VideoURLPanelObject data = new VideoURLPanelObject();

        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;
        data.SetXY(pos.x, pos.y);
        data.SetScale(scale.x, scale.y);
        data.SetURL(GetComponent<VideoPlayer>().url);

        return data;
    }
}
