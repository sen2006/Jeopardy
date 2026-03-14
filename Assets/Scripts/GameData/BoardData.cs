using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardData : IByteSerialization {
    [SerializeField]
    List<CategoryData> categories = new List<CategoryData>();
    public QuestionData getQuestionFor(int boardX, int boardY) {
        return categories[boardX].GetQuestion(boardY);
    }

    public int getBoardWidth() {
        return categories.Count;
    }

    public int getBoardHeight() {
        int longest = 0;
        foreach (CategoryData category in categories) {
            int length = category.Size();
            if (length > longest) 
                longest = length;
        }
        return longest;
    }

    public void Deserialize(Packet pPacket) {
        categories = pPacket.ReadList<CategoryData>();
    }

    public void Serialize(Packet pPacket) {
        pPacket.WriteList(categories);
    }

    internal void loadToScene(GameObject renderParent) {
        throw new NotImplementedException();
    }

    internal void SetupPanels(int width, int height) {
        categories = new List<CategoryData>();
        int w = 0;
        while (w < width) {
            CategoryData category = new CategoryData();
            category.SetName("Category-" + w);
            category.SetupCategory(height);
            categories.Add(category);
            w++;
        }
    }

    internal CategoryData GetCategory(int index) {
        return categories[index];
    }
}

public struct BoardSaveData {
    public List<CategoryData> categories;
}
