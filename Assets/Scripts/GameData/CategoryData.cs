using System;
using System.Collections.Generic;

public class CategoryData : IByteSerialization {
    string name = "";
    List<QuestionData> questions = new List<QuestionData>();
    public static int defautlCashAmount = 100;

    public QuestionData GetQuestion(int boardY) { return questions[boardY]; }

    public void SetupCategory(int size) {
        int i = 0;
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

    internal string GetName() {
        return name;
    }
}