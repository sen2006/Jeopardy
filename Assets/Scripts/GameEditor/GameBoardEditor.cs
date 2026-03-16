using System;
using System.IO;
using TMPro;
using UnityEngine;

public class GameBoardEditor : MonoBehaviour, IPanelLoader {
    public static GameBoardEditor singleton;

    [SerializeField] GameData gameData;
    [SerializeField] GameObject boardRenderParent;
    [SerializeField] GameObject panelRenderParent;
    [SerializeField] GameObject boardEditorButtons;
    [SerializeField] GameObject panelEditorButtons;
    [SerializeField] GameObject categoryPrefab;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] TMP_InputField saveFileNameInput;
    [SerializeField] TMP_InputField boardTitleInput;
    [SerializeField] TextMeshProUGUI questionCashDisplay;
    static GameObject PanelButton;

    QuestionData currentlyLoadedQuestion;
    int selectedQuestionPanelIndex = 0;
    int selectedBoardIndex = 0;

    public void Start() {
        if (singleton != null) throw new Exception("Singleton Already Exists");
        singleton = this;

        //if (PanelButton == null)
        //    PanelButton = (GameObject)Resources.Load("Assets/Prefabs/EdditorPanelButton");
        gameData = new GameData();
        //gameData.AddNewBoard();
        //gameData.GetBoard(0).SetupPanels(5, 5);
        //SaveAndRenderBoard(0);

        boardEditorButtons.SetActive(true);
        panelEditorButtons.SetActive(false);

        boardTitleInput.onValueChanged.AddListener(UpdateBoardSaveName);

        questionCashDisplay.text = "";
    }

    private void Update() {
        if (gameData == null || gameData.GetBoardCount() <= 0)
            boardTitleInput.text = "";
        if (currentlyLoadedQuestion != null)
            questionCashDisplay.text = currentlyLoadedQuestion.GetRewardCashAmount()+"$";
    }

    public void CreateBoard(int w, int h) {
        gameData.AddNewBoard();
        int boardIndex = gameData.GetBoardCount() - 1;
        gameData.GetBoard(boardIndex).SetupPanels(w, h);
        SaveAndRenderBoard(boardIndex);
    }

    public void DeleteThisBoard() {
        gameData.DeleteBoard(selectedBoardIndex);
        selectedBoardIndex--;
        if (selectedBoardIndex < 0)
            selectedBoardIndex = 0;
        if (gameData.GetBoardCount() > 0)
            SaveAndRenderBoard(selectedBoardIndex);
        else 
            DestroyAllChildren();
    }

    public void GetNextBoard() {
        if (gameData.GetBoardCount() == 0) return;
        selectedBoardIndex++;
        if (selectedBoardIndex >= gameData.GetBoardCount()) 
            selectedBoardIndex = gameData.GetBoardCount() - 1;
        SaveAndRenderBoard(selectedBoardIndex);
    }

    public void GetPreviosBoard() {
        if (gameData.GetBoardCount() == 0) return;
        selectedBoardIndex--;
        if (selectedBoardIndex < 0) 
            selectedBoardIndex = 0;
        SaveAndRenderBoard(selectedBoardIndex);
    }

    public void ReturnFromPanel() {
        SaveAndRenderBoard(selectedBoardIndex);
    }

    public void SaveAndRenderBoard(int boardIndex) {
        Save();
        OpenBoard(boardIndex);
    }

    public void OpenBoard(int index) {
        DestroyAllChildren();
        SetupBoardButtons(gameData.GetBoard(index));
        SetBoardName(gameData.GetBoard(index).GetName());
    }

    private void SetBoardName(string name) {
        boardTitleInput.text = name;
    }

    public void UpdateBoardSaveName(string name) {
        if (gameData != null && gameData.GetBoardCount() > 0)
            gameData.GetBoard(selectedBoardIndex).SetName(name);
    }

    private void SetupBoardButtons(BoardData board) {
        int width = board.getBoardWidth();
        int height = board.getBoardHeight();
        int w = 0;
        while (w < width) {
            GameObject newCategory = Instantiate(categoryPrefab, Vector3.zero, Quaternion.identity);
            EditorCategory editor = newCategory.AddComponent<EditorCategory>();
            newCategory.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = board.GetCategory(w).GetName();
            editor.SetCategory(board.GetCategory(w));
            newCategory.transform.SetParent(boardRenderParent.transform);
            newCategory.transform.localScale = Vector3.one;
            int h = 0;
            while (h < height) {
                GameObject newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
                newButton.transform.SetParent(newCategory.transform.GetChild(0));
                PanelButton panelButton = newButton.GetComponent<PanelButton>();
                panelButton.SetQuestion(board.getQuestionFor(w,h));
                panelButton.SetPanelLoader(this);
                panelButton.SetCashText(board.getQuestionFor(w, h).cashAmount);
                panelButton.transform.localScale = Vector3.one;
                h++;
            }
            w++;
        }
    }

    private void DestroyAllChildren() {
        foreach (Transform child in boardRenderParent.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in panelRenderParent.transform) {
            Destroy(child.gameObject);
        }
    }

    private void Save() {
        if (currentlyLoadedQuestion != null) {
            SavePanelToQuestion();
            currentlyLoadedQuestion = null;
        }
    }

    public void LoadPanel(PanelData panelData) {
        DestroyAllChildren();
        panelData.loadToEditorScene(panelRenderParent);
    }

    public void SetLoadedQuestion(QuestionData data) {
        selectedQuestionPanelIndex = 0;
        LoadPanel(data.getPanel(0));
        boardEditorButtons.SetActive(false);
        panelEditorButtons.SetActive(true);
        currentlyLoadedQuestion = data;
    }

    public void SavePanelToQuestion() {
        PanelData data = new PanelData();

        foreach (Transform child in panelRenderParent.transform) {
            EditorPanelObject panelObject = child.GetComponent<EditorPanelObject>();
            if (panelObject != null) {
                data.AddObject(panelObject.GetDataObject());
            }
        }
        
        currentlyLoadedQuestion.savePanelData(data, selectedQuestionPanelIndex);
    }

    public void GetNextPanelInQuestion() {
        if (currentlyLoadedQuestion == null) return;
        SavePanelToQuestion();
        selectedQuestionPanelIndex++;
        if (selectedQuestionPanelIndex >= currentlyLoadedQuestion.GetPanelCount()) {
            selectedQuestionPanelIndex = currentlyLoadedQuestion.GetPanelCount() - 1;
        }
        LoadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }

    public void GetPreviosPanelInQuestion() {
        if (currentlyLoadedQuestion == null) return;
        SavePanelToQuestion();
        selectedQuestionPanelIndex--;
        if (selectedQuestionPanelIndex < 0) {
            selectedQuestionPanelIndex = 0;
        }
        LoadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }

    public void SaveToFile() {
        GameSaveData save = gameData.Save();
        SaveSystem.Save(saveFileNameInput.text, save);
        //ShowExplorer(saveFileNameInput.text);
    }

    public void LoadFromFile() {
        gameData.Load(SaveSystem.Load(saveFileNameInput.text));
        OpenBoard(0);
    }

    public static void ShowExplorer() {
        string path = Path.Combine(Application.persistentDataPath, "saves");
        EnsureDirectoryExists(path);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(path);
#else
        OpenFolder(path);
#endif
    }

    public static void ShowExplorer(string saveName) {
        string path = Path.Combine(Application.persistentDataPath, "saves", saveName);
        EnsureDirectoryExists(path);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(path);
#else
        OpenFolder(path);
#endif
    }

    private static void EnsureDirectoryExists(string path) {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private static void OpenFolder(string path) {
        // Windows
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            System.Diagnostics.Process.Start("explorer.exe", path);
        // macOS
        else if (Application.platform == RuntimePlatform.OSXPlayer)
            System.Diagnostics.Process.Start("open", path);
        // Linux (may vary)
        else if (Application.platform == RuntimePlatform.LinuxPlayer)
            System.Diagnostics.Process.Start("xdg-open", path);
    }

    public void AddPanelToQuestion() {
        currentlyLoadedQuestion.AddPanel();
    }

    public void RemoveCurrentPanel() {
        if (currentlyLoadedQuestion.GetPanelCount() <= 1) return;
        currentlyLoadedQuestion.RemovePanel(selectedQuestionPanelIndex);

        if (selectedQuestionPanelIndex >= currentlyLoadedQuestion.GetPanelCount())
            selectedQuestionPanelIndex = currentlyLoadedQuestion.GetPanelCount() - 1;
        LoadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }

    public void ClearCurrentPanel() {
        currentlyLoadedQuestion.ClearPanel(selectedQuestionPanelIndex);
        LoadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }
}
