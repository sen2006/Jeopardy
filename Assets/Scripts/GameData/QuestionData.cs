


using System.Collections.Generic;
using UnityEngine;

public class QuestionData : IByteSerialization {
    public int cashAmount { get; private set; }
    List<PanelData> panels = new List<PanelData>();

    public QuestionData() { 
        panels.Add(new PanelData());
        panels.Add(new PanelData());
        panels.Add(new PanelData());
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
}