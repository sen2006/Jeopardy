using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardData : IByteSerialization {
    [SerializeField]
    List<boardCategory> categories = new List<boardCategory>();
    public QuestionData getQuestionFor(int boardX, int boardY) {
        return categories[boardX].GetQuestion(boardY);
    }

    public int getBoardWidth() {
        return categories.Count;
    }

    public int getBoardHeight() {
        int longest = 0;
        foreach (boardCategory category in categories) {
            int length = category.Size();
            if (length > longest) 
                longest = length;
        }
        return longest;
    }

    public void Deserialize(Packet pPacket) {
        categories = pPacket.ReadList<boardCategory>();
    }

    public void Serialize(Packet pPacket) {
        pPacket.WriteList(categories);
    }

    internal void loadToScene(GameObject renderParent) {
        throw new NotImplementedException();
    }

    internal void SetupPanels(int width, int height) {
        categories = new List<boardCategory>();
        int w = 0;
        while (w < width) {
            boardCategory category = new boardCategory();
            category.SetName("Category-" + w);
            category.SetupCategory(height);
            categories.Add(category);
            w++;
        }
    }
}

class boardCategory : IByteSerialization {
    string name;
    List<QuestionData> questions = new List<QuestionData>();
    public static int defautlCashAmount = 100;

    public QuestionData GetQuestion(int boardY) { return questions[boardY]; }

    public void SetupCategory(int size) {
        int i =0;
        while (i < size) {
            QuestionData question = new QuestionData();
            question.SetCash((i + 1) * defautlCashAmount);
            questions.Add(question);
            i++;
        }
    }

    public int Size() {
        return questions.Count;
    }

    public void Deserialize(Packet pPacket) {
        name = pPacket.ReadString();
        questions = pPacket.ReadList<QuestionData>();
    }

    public void Serialize(Packet pPacket) {
        pPacket.Write(name);
        pPacket.WriteList(questions);
    }

    internal void SetName(string name) {
        this.name = name;
    }
}
