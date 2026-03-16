using PurrNet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreCard : MonoBehaviour
{
    [SerializeField] int cash = 0;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI buzzerCountDisplay;
    [SerializeField] RawImage image;
    [SerializeField] Button increaseCash;
    [SerializeField] Button decreaseCash;
    PlayerID playerID;

    private void Start() {
        increaseCash.onClick.AddListener(addCashOnClick);
        decreaseCash.onClick.AddListener(removeCashOnClick);
    }

    private void Update() {
        score.text = cash + "$";
        if (GameManager.HasBuzzed(playerID)) {
            buzzerCountDisplay.text = (GameManager.GetBuzzedPlace(playerID)+1) + "";
        } else {
            buzzerCountDisplay.text = "";
        }
    }

    public void SetName(string name) {
        nameText.text = name;
    }

    public void SetCash(int cash) {
        this.cash = cash;
    }

    public void AddCash(int cash) {
        this.cash += cash;
    }

    public void SetImage(Texture texture) {
        image.texture = texture;
    }

    private void addCashOnClick() {
        cash += 100;
    }

    private void removeCashOnClick() {
        cash -= 100;
    }

    public void LinkPlayer(PlayerID id) {
        playerID = id;
    }
}
