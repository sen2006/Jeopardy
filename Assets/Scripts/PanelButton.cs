using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button button;

    QuestionData data;
    IPanelLoader loader;

    public void Start() {
        button.onClick.AddListener(OnClick);
    }

    public void SetCashText(int amount) {
        text.text = amount + "$";
    }

    public void OnClick() {
        loader.SetLoadedQuestion(data);
    }

    public void SetQuestion(QuestionData panelData) {
        data = panelData;
    }

    public void SetPanelLoader(IPanelLoader loader) {
        this.loader = loader;
    }
}
