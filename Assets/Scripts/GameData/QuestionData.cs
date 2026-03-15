


using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestionData : ISaveSerialization<QuestionSaveData> {
    public int cashAmount { get; private set; }
    int ID = 0;
    static int lastID = 1;
    List<PanelData> panels = new List<PanelData>();

    public QuestionData() {
        AddPanel();
        ID = lastID++; // just here to make sure they are always seen as different objects

    }

    public int GetRewardCashAmount() {
        return cashAmount;
    }

    
    public void SetCash(int cash) {
        cashAmount = cash;
    }

    internal PanelData getPanel(int index) {
        return panels[index];
    }

    internal void savePanelData(PanelData data, int index) {
        panels[index] = data;
    }

    internal int GetPanelCount() {
        return panels.Count;
    }

    public void AddPanel() {
        panels.Add(new PanelData());
    }

    public void RemovePanel(int index) {
        panels.RemoveAt(index);
    }

    public void ClearPanel(int index) {
        panels[index] = new PanelData();
    }

    public QuestionSaveData Save() {
        QuestionSaveData save = new QuestionSaveData();
        save.cashAmount = cashAmount;
        save.panels = ISaveSerialization<PanelSaveData>.ConvertListToSave(panels);
        return save;
    }

    public void Load(QuestionSaveData saveData) {
        cashAmount = saveData.cashAmount;
        panels = ISaveSerialization<PanelSaveData>.ConvertListFromSave<PanelData>(saveData.panels, typeof(PanelData));
    }
}

[Serializable]
public struct QuestionSaveData {
    public int cashAmount;
    public List<PanelSaveData> panels;
}