using System;
using System.Collections.Generic;

public class CategoryData : ISaveSerialization<CategorySaveData> {
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

    public void SetName(string name) {
        this.name = name;
    }

    public string GetName() {
        return name;
    }

    public CategorySaveData Save() {
        CategorySaveData save = new CategorySaveData();
        save.name = name;
        save.questions = ISaveSerialization<QuestionSaveData>.ConvertListToSave(questions);
        return save;    
    }

    public void Load(CategorySaveData saveData) {
        name = saveData.name;
        questions = ISaveSerialization<QuestionSaveData>.ConvertListFromSave<QuestionData>(saveData.questions, typeof(QuestionData));
    }
}

[Serializable]
public struct CategorySaveData {
    public string name;
    public List<QuestionSaveData> questions;
}