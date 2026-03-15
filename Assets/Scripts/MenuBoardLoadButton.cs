using System;
using TMPro;
using UnityEngine;

public class MenuBoardLoadButton : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI debugText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        debugText.text = "Board Loaded :None\n---";
    }

    public void TryLoad() {
        try {
            PersistentBoardSave.LoadFromFile(nameInput.text);
            debugText.text = "Board Loaded :" + PersistentBoardSave.GetGameData().GetGameName() + "\n---";
        } catch (Exception e) {
            PersistentBoardSave.ScrapSave();
            debugText.text = "Board Loaded :None\nBoard Not Found";
            Debug.LogException(e);
        }
    }
}
