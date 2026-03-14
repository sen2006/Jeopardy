using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class EditorImageButton : MonoBehaviour {
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
                TryLoadImage();
                break;
            }
        }
    }

    private void TryLoadImage() {
        string path = textInputField.text;
        path = path.Replace("\"", "");
        if (File.Exists(path)) {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            CreateUI(tex);
        } else {
            Debug.Log("file does not exist");
        }
    }

    private void CreateUI(Texture2D texture) {
        GameObject obj = new GameObject("ClipboardImage", typeof(RectTransform), typeof(RawImage), typeof(EditorImagePanelObject), typeof(DragableUI));

        obj.transform.SetParent(canvas.transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(texture.width, texture.height);

        RawImage image = obj.GetComponent<RawImage>();
        image.texture = texture;
    }
}