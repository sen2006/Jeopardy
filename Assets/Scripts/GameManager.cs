using PurrNet;
using Steamworks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour, IPanelLoader {
    // runs 90% on the server
    // host = server
    // host + players = observer
    bool isPlaying = false;

    [SerializeField] GameObject preStartStatus;
    [Header("Host Stuff")]
    [SerializeField] GameObject hostObjects;
    [SerializeField] GameObject hostObjectsPlayMode;
    [SerializeField] GameObject gameBoardRenderParent;
    [SerializeField] GameObject gamePanelRenderParent;
    [SerializeField] GameObject playerCardParent;

    [SerializeField] GameObject panelButtons;
    [SerializeField] GameObject boardButtons;
    [SerializeField] GameObject alwaysButtons;

    [SerializeField] GameObject categoryPrefab;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject playerCardPrefab;

    QuestionData currentlyLoadedQuestion;
    int selectedQuestionPanelIndex = 0;
    int selectedBoardIndex = 0;
    static List<PlayerID> buzzedPlayers = new();
    List<QuestionData> usedButtons = new(); //(board, w, h)

    [Header("Player Stuff")]
    [SerializeField] GameObject playerObjects;
    [SerializeField] GameObject playerObjectsPlayMode;
    [SerializeField] Button buzzer;

    [Header("Debuging")]
    [SerializeField] bool forceHost = false;
    [SerializeField] bool forcePlayer = false;

    private bool isGameGost() {
        if (forcePlayer) return false;
        else return isHost || forceHost;
    }

    private void Start() {
        preStartStatus.SetActive(true);
        hostObjects.SetActive(false);
        hostObjectsPlayMode.SetActive(false);
        playerObjects.SetActive(false);
        playerObjectsPlayMode.SetActive(false);

        panelButtons.SetActive(false);
        boardButtons.SetActive(false);
        alwaysButtons.SetActive(false);
    }

    private void Update() {
        preStartStatus.SetActive(!isPlaying);
        hostObjects.SetActive(!isPlaying && isGameGost());
        playerObjects.SetActive(!isPlaying && !isGameGost());
        hostObjectsPlayMode.SetActive(isPlaying && isGameGost());
        playerObjectsPlayMode.SetActive(isPlaying && !isGameGost());

        if (isGameGost()) {
            alwaysButtons.SetActive(isPlaying);
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame) {
                GetNextPanelInQuestion();
            }
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame) {
                GetPreviosPanelInQuestion();
            }
        }
    }

    public void HostStartButton() {
        if (isGameGost()) {
            StartGame();
        }
    }

    [ObserversRpc]
    private void StartGame() {
        isPlaying = true;

        if (isGameGost()) {
            OpenBoard(0);
            CreatePlayerCards();
            boardButtons.SetActive(true);
        }
    }

    /// <summary>
    /// called by client pressing buzzer
    /// </summary>
    public void Buzz() {
        //if (isGameGost()) throw new System.Exception("Buzzed as Host");
        //TODO: check if buzzer is locked
        BuzzRPC();
    }

    [ObserversRpc]
    private void BuzzRPC(RPCInfo info = default) {
        PlayerID buzzerPlayer = info.sender;

        if (isGameGost()) {
            if (!buzzedPlayers.Contains(buzzerPlayer)) {
                buzzedPlayers.Add(buzzerPlayer);
                LockBuzzer(info.sender);
                //TODO: Play Buzz Sound on all clients
            }
        }
        
        string playerName = SteamFriends.GetFriendPersonaName(new CSteamID((ulong)buzzerPlayer.id));

        Debug.Log(SteamIDStorage.playerNames[buzzerPlayer] + " buzzed");
    }

    [TargetRpc]
    private void LockBuzzer(PlayerID target) {
        buzzer.interactable = false;
    }

    [ObserversRpc]
    private void UnlockBuzzers() {
        buzzer.interactable = true;
    }

    public static bool HasBuzzed(PlayerID player) {
        return buzzedPlayers.Contains(player);
    }

    public static int GetBuzzedPlace(PlayerID player) {
        return buzzedPlayers.IndexOf(player);
    }

    public void ResetBuzzers() {
        buzzedPlayers.Clear();
        UnlockBuzzers();
    }

    /** Game Host Stuff
     */

    public void ReturnFromPanel() {
        OpenBoard(selectedBoardIndex);
        currentlyLoadedQuestion = null;
        selectedQuestionPanelIndex = 0;
    }

    public void OpenBoard(int index) {
        DestroyAllChildren();
        SetupBoardButtons(PersistentBoardSave.GetGameData().GetBoard(index));
        panelButtons.SetActive(false);
        boardButtons.SetActive(true);
    }

    public void CreatePlayerCards() {
        foreach (Transform child in playerCardParent.transform) {
            Destroy(child.gameObject);
        }

        foreach (PlayerID player in networkManager.players) {
            if (player == networkManager.localPlayer) continue;


            try {
                GameObject card = Instantiate(playerCardPrefab, playerCardParent.transform);
                PlayerScoreCard cardData = card.GetComponent<PlayerScoreCard>();

                cardData.SetCash(GameData.GetStartingCash());
                cardData.SetName(SteamIDStorage.playerNames[player]);
                cardData.SetImage(SteamIDStorage.playerAvatars[player]);
                cardData.LinkPlayer(player);
            } catch { }
        }
    }

    private void SetupBoardButtons(BoardData board) {
        int width = board.getBoardWidth();
        int height = board.getBoardHeight();
        int w = 0;
        while (w < width) {
            GameObject newCategory = Instantiate(categoryPrefab, Vector3.zero, Quaternion.identity);
            newCategory.transform.SetParent(gameBoardRenderParent.transform);
            newCategory.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PersistentBoardSave.GetGameData().GetBoard(selectedBoardIndex).GetCategory(w).GetName();
            newCategory.transform.localScale = Vector3.one;
            int h = 0;
            while (h < height) {
                GameObject newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
                newButton.transform.SetParent(newCategory.transform.GetChild(0));
                PanelButton panelButton = newButton.GetComponent<PanelButton>();
                QuestionData question = board.getQuestionFor(w, h);

                panelButton.setQuestion(question);
                panelButton.setPanelLoader(this);
                panelButton.setCashText(question.cashAmount);
                if (usedButtons.Contains(question)) {
                    panelButton.GetComponent<Image>().color = Color.gray;
                }
                panelButton.transform.localScale = Vector3.one;
                h++;
            }
            w++;
        }
    }

    private void DestroyAllChildren() {
        foreach (Transform child in gameBoardRenderParent.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in gamePanelRenderParent.transform) {
            Destroy(child.gameObject);
        }
    }

    public void loadPanel(PanelData panelData) {
        DestroyAllChildren();
        panelData.loadToScene(gamePanelRenderParent);
        panelButtons.SetActive(true);
        boardButtons.SetActive(false);
    }

    public void setLoadedQeastion(QuestionData data) {
        selectedQuestionPanelIndex = 0;
        loadPanel(data.getPanel(0));
        currentlyLoadedQuestion = data;
        usedButtons.Add(data);
    }

    private void GetNextPanelInQuestion() {
        if (currentlyLoadedQuestion == null) return;
        selectedQuestionPanelIndex++;
        if (selectedQuestionPanelIndex >= currentlyLoadedQuestion.GetPanelCount()) {
            selectedQuestionPanelIndex = currentlyLoadedQuestion.GetPanelCount() - 1;
        }
        loadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }

    private void GetPreviosPanelInQuestion() {
        if (currentlyLoadedQuestion == null) return;
        selectedQuestionPanelIndex--;
        if (selectedQuestionPanelIndex < 0) {
            selectedQuestionPanelIndex = 0;
        }
        loadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }
}
