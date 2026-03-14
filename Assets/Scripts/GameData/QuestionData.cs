


using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestionData : IByteSerialization {
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

    public void Deserialize(Packet pPacket) {
        cashAmount = pPacket.ReadInt();
        panels = pPacket.ReadList<PanelData>();
    }

    public void Serialize(Packet pPacket) {
        pPacket.Write(cashAmount);
        pPacket.WriteList(panels);
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

    internal void RemovePanel(int index) {
        panels.RemoveAt(index);
    }

    internal void ClearPanel(int index) {
        panels[index] = new PanelData();
    }
}