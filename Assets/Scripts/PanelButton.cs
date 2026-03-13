using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    public void setCashText(int amount) {
        text.text = amount + "$";
    }

    public void OnClick() {
        loader.setLoadedQeastion(data);
    }

    public void setQuestion(QuestionData panelData) {
        data = panelData;
    }

    public void setPanelLoader(IPanelLoader loader) {
        this.loader = loader;
    }
}
