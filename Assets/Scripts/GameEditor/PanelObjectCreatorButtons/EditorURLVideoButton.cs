using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class EditorURLVideoButton : MonoBehaviour {
    enum ButtonState {
        Unpressed,
        Waiting
    }

    [SerializeField] Button button;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject inputField;
    [SerializeField] TMP_InputField textInputField;
    [SerializeField] ButtonState state;

    private void Start() {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        switch (state) {
            case ButtonState.Unpressed: {
                textInputField.text = "";
                inputField.SetActive(true);
                state = ButtonState.Waiting;
                break;
            }
            case ButtonState.Waiting: {
                inputField.SetActive(false);
                state = ButtonState.Unpressed;
                TryLoad();
                break;
            }
        }
    }

    private void TryLoad() {
        GameObject obj = new GameObject("VideoURL",
            typeof(RectTransform),
            typeof(RawImage),
            typeof(VideoPlayer),
            typeof(DragableUI)
        );

        obj.transform.SetParent(canvas.transform, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(0,0);
        rect.sizeDelta = new Vector2(500, 300);
        rect.localScale = new Vector2(1, 1);
        RawImage image = obj.GetComponent<RawImage>();
        VideoPlayer player = obj.GetComponent<VideoPlayer>();
        RenderTexture rt = new RenderTexture(512, 512, 0);
        image.texture = rt;
        player.renderMode = VideoRenderMode.RenderTexture;
        player.targetTexture = rt;
        player.url = textInputField.text;
        player.playOnAwake = false;
        player.isLooping = true;
    }

}