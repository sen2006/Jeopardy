using System;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    static GameObject PanelButton;

    QuestionData currentlyLoadedQuestion;
    int selectedQuestionPanelIndex = 0;

    public void Start() {
        if (singleton != null) throw new Exception("Singleton Already Exists");
        singleton = this;

        //if (PanelButton == null)
        //    PanelButton = (GameObject)Resources.Load("Assets/Prefabs/EdditorPanelButton");
        gameData = new GameData();
        gameData.AddNewBoard();
        gameData.GetBoard(0).SetupPanels(5, 5);
        SaveAndRenderBoard();

        boardEditorButtons.SetActive(true);
        panelEditorButtons.SetActive(false);
    }

    public void SaveAndRenderBoard() {
        Save();
        OpenBoard(0);
    }

    public void OpenBoard(int index) {
        DestroyAllChildren();
        SetupBoardButtons(gameData.GetBoard(index));
    }

    private void SetupBoardButtons(BoardData board) {
        int width = board.getBoardWidth();
        int height = board.getBoardHeight();
        int w = 0;
        while (w < width) {
            GameObject newCategory = Instantiate(categoryPrefab, Vector3.zero, Quaternion.identity);
            newCategory.transform.SetParent(boardRenderParent.transform);
            newCategory.transform.localScale = Vector3.one;
            int h = 0;
            while (h < height) {
                GameObject newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
                newButton.transform.SetParent(newCategory.transform);
                PanelButton panelButton = newButton.GetComponent<PanelButton>();
                panelButton.setQuestion(board.getQuestionFor(w,h));
                panelButton.setPanelLoader(this);
                panelButton.setCashText(board.getQuestionFor(w, h).cashAmount);
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

    public void loadPanel(PanelData panelData) {
        DestroyAllChildren();
        panelData.loadToEditorScene(panelRenderParent);
    }

    public void setLoadedQeastion(QuestionData data) {
        selectedQuestionPanelIndex = 0;
        loadPanel(data.getPanel(0));
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
        SavePanelToQuestion();
        selectedQuestionPanelIndex++;
        if (selectedQuestionPanelIndex >= currentlyLoadedQuestion.GetPanelCount()) {
            selectedQuestionPanelIndex = currentlyLoadedQuestion.GetPanelCount() - 1;
        }
        loadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }

    public void GetPreviosPanelInQuestion() {
        SavePanelToQuestion();
        selectedQuestionPanelIndex--;
        if (selectedQuestionPanelIndex < 0) {
            selectedQuestionPanelIndex = 0;
        }
        loadPanel(currentlyLoadedQuestion.getPanel(selectedQuestionPanelIndex));
    }

    public void SaveToFile() {
        string path = PersistentBoardSave.savePath + "/" + saveFileNameInput.text + ".gameBoard";

        if (currentlyLoadedQuestion != null)
            SavePanelToQuestion();
        Packet packet = new Packet();
        gameData.Serialize(packet);
        if (!Directory.Exists(Path.GetDirectoryName(path)))
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, packet.GetBytes());
        Debug.Log("Saved To: " + path);
        //ShowExplorer(saveFileNameInput.text);
    }

    public void LoadFromFile() {
        string path = PersistentBoardSave.savePath + "/" + saveFileNameInput.text + ".gameBoard";

        Packet packet = new Packet(File.ReadAllBytes(path));
        gameData.Deserialize(packet);
        Debug.Log("Loaded From: " + PersistentBoardSave.savePath +"/" + saveFileNameInput.text+".gameBoard");
        OpenBoard(0);
    }

    public static void ShowExplorer() {
        if (!Directory.Exists(Path.GetDirectoryName(PersistentBoardSave.savePath + "/")))
            Directory.CreateDirectory(Path.GetDirectoryName(PersistentBoardSave.savePath + "/"));
        EditorUtility.RevealInFinder(Application.persistentDataPath+ "/saves");
    }

    public static void ShowExplorer(string saveName) {
        if (!Directory.Exists(Path.GetDirectoryName(PersistentBoardSave.savePath +"/")))
            Directory.CreateDirectory(Path.GetDirectoryName(PersistentBoardSave.savePath +"/"));
        EditorUtility.RevealInFinder(Application.persistentDataPath + "/saves/" + saveName);
    }
}
