using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateBoardButton : MonoBehaviour
{
    enum ButtonState {
        UnPressed,
        Waiting
    }

    Button button;
    [SerializeField] TMP_InputField widthInput;
    [SerializeField] TMP_InputField heightInput;
    [SerializeField] ButtonState state = ButtonState.UnPressed;

    public void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }

    private void Update() {
        widthInput.gameObject.SetActive(state == ButtonState.Waiting);
        heightInput.gameObject.SetActive(state == ButtonState.Waiting);
    }

    private void onClick() {
        switch (state) {
            case ButtonState.UnPressed:
                state = ButtonState.Waiting;
                break;
            case ButtonState.Waiting:
                state = ButtonState.UnPressed;
                int w = int.Parse(widthInput.text);
                int h = int.Parse(heightInput.text);
                if (w > 0 && h > 0)
                    GameBoardEditor.singleton.CreateBoard(w, h);
                break;
        }
    }

}
