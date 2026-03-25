using PurrNet;
using Steamworks;
using System;
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
    [SerializeField] GameObject nextGamePanelRenderParent;
    [SerializeField] GameObject playerCardParent;

    [SerializeField] GameObject panelButtons;
    [SerializeField] GameObject boardButtons;
    [SerializeField] GameObject alwaysButtons;

    [SerializeField] GameObject categoryPrefab;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject playerCardPrefab;

    [SerializeField] BuzzerSounds soundHandeler;

    [SerializeField] TextMeshProUGUI boardTitleDisplay;
    [SerializeField] TextMeshProUGUI questionCashDisplay;

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
    internal static GameManager singleton;

    private bool isGameGost() {
        if (forcePlayer) return false;
        else return isHost || forceHost;
    }

    private void Start() {
        singleton = this;
        preStartStatus.SetActive(true);
        hostObjects.SetActive(false);
        hostObjectsPlayMode.SetActive(false);
        playerObjects.SetActive(false);
        playerObjectsPlayMode.SetActive(false);

        panelButtons.SetActive(false);
        boardButtons.SetActive(false);
        alwaysButtons.SetActive(false);

        questionCashDisplay.text = "";
    }

    private void Update() {
        preStartStatus.SetActive(!isPlaying);
        hostObjects.SetActive(!isPlaying && isGameGost());
        playerObjects.SetActive(!isPlaying && !isGameGost());
        hostObjectsPlayMode.SetActive(isPlaying && isGameGost());
        playerObjectsPlayMode.SetActive(isPlaying && !isGameGost());

        if (isGameGost()) {
            alwaysButtons.SetActive(isPlaying);
            if (currentlyLoadedQuestion != null) {
                if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame) {
                    GetNextPanelInQuestion();
                }
                if (Keyboard.current.leftArrowKey.wasPressedThisFrame) {
                    GetPreviosPanelInQuestion();
                }
            } else {
                if (Keyboard.current.rightArrowKey.wasPressedThisFrame) {
                    GetNextBoard();
                }
                if (Keyboard.current.leftArrowKey.wasPressedThisFrame) {
                    GetPreviosBoard();
                }
            }
            if (currentlyLoadedQuestion != null)
                questionCashDisplay.text = currentlyLoadedQuestion.GetRewardCashAmount()+"$";
        } else {
            // buzz using the keyboard
            if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                Buzz();
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
            if (Display.displays.Length > 1) {
                Display.displays[1].Activate();
            }
        }
    }

    /// <summary>
    /// called by client pressing buzzer
    /// </summary>
    public void Buzz() {
        BuzzRPC();
    }

    [ObserversRpc]
    private void BuzzRPC(RPCInfo info = default) {
        PlayerID buzzerPlayer = info.sender;

        if (isGameGost()) {
            if (!buzzedPlayers.Contains(buzzerPlayer)) {
                buzzedPlayers.Add(buzzerPlayer);
                LockBuzzer(buzzerPlayer);
                PlayBuzzAudio(SteamIDStorage.playerSteamIDs[buzzerPlayer]);
            }
        }
        
        string playerName = SteamFriends.GetFriendPersonaName(new CSteamID((ulong)buzzerPlayer.id));

        Debug.Log(SteamIDStorage.playerNames[buzzerPlayer] + " buzzed");
    }

    [ObserversRpc]
    private void PlayBuzzAudio(ulong steamID, RPCInfo info = default) {
        soundHandeler.PlayBuzzSound(steamID);
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
        currentlyLoadedQuestion = null;
        selectedQuestionPanelIndex = 0;
        OpenBoard(selectedBoardIndex);
        foreach (Transform child in nextGamePanelRenderParent.transform) {
            Destroy(child.gameObject);
        }
    }

    public void OpenBoard(int index) {
        DestroyAllChildren();
        SetupBoardButtons(PersistentBoardSave.GetGameData().GetBoard(index));
        panelButtons.SetActive(false);
        boardButtons.SetActive(true);
        boardTitleDisplay.text = PersistentBoardSave.GetGameData().GetBoard(index).GetName();
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

                panelButton.SetQuestion(question);
                panelButton.SetPanelLoader(this);
                panelButton.SetCashText(question.cashAmount);
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

    public void LoadPanel(PanelData panelData) {
        DestroyAllChildren();
        panelData.loadToScene(gamePanelRenderParent);
        panelButtons.SetActive(true);
        boardButtons.SetActive(false);
    }

    public void loadNextPanel(PanelData panelData) {
        foreach (Transform child in nextGamePanelRenderParent.transform) {
            Destroy(child.gameObject);
        }
        panelData.loadToScene(nextGamePanelRenderParent);
    }

    public void SetLoadedQuestion(QuestionData data) {
        selectedQuestionPanelIndex = 0;
        LoadPanel(data.getPanel(0));
        if (data.GetPanelCount() > 1) {
            loadNextPanel(data.getPanel(1));
        }
        currentlyLoadedQuestion = data;
        usedButtons.Add(data);
    }

    private void GetNextPanelInQuestion() {
        if (currentlyLoadedQuestion == null) return;
        selectedQuestionPanelIndex++;
        if (selectedQuestionPanelIndex >= currentlyLoadedQuestion.GetPanelCount()) {
            selectedQuestionPanelIndex = currentlyLoadedQuestion.GetPanelCount() - 1;
        }
        LoadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
        if (currentlyLoadedQuestion.GetPanelCount() > selectedQuestionPanelIndex+1)
            loadNextPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex+1));
    }

    private void GetPreviosPanelInQuestion() {
        if (currentlyLoadedQuestion == null) return;
        selectedQuestionPanelIndex--;
        if (selectedQuestionPanelIndex < 0) {
            selectedQuestionPanelIndex = 0;
        }
        LoadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
        if (currentlyLoadedQuestion.GetPanelCount() > selectedQuestionPanelIndex + 1)
            loadNextPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex + 1));
    }

    private void GetNextBoard() {
        selectedBoardIndex++;
        if (selectedBoardIndex >= PersistentBoardSave.GetGameData().GetBoardCount()) {
            selectedBoardIndex = PersistentBoardSave.GetGameData().GetBoardCount() - 1;
        }
        OpenBoard(selectedBoardIndex);
    }

    private void GetPreviosBoard() {
        selectedBoardIndex--;
        if (selectedBoardIndex < 0) {
            selectedBoardIndex = 0;
        }
        OpenBoard(selectedBoardIndex);
    }

    internal int GetCashChange() {
        if (Keyboard.current.leftCtrlKey.isPressed)
            return 1;
        if (Keyboard.current.leftAltKey.isPressed)
            return 10;
        if (currentlyLoadedQuestion == null || Keyboard.current.leftShiftKey.isPressed)
            return 100;
        return currentlyLoadedQuestion.GetRewardCashAmount();
    }
}
