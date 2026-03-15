using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardData : ISaveSerialization<BoardSaveData> {
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

    public void loadToScene(GameObject renderParent) {
        throw new NotImplementedException();
    }

    public void SetupPanels(int width, int height) {
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

    public CategoryData GetCategory(int index) {
        return categories[index];
    }


    public BoardSaveData Save() {
        BoardSaveData saveData = new BoardSaveData();
        saveData.categories = ISaveSerialization<CategorySaveData>.ConvertListToSave(categories);
        return saveData;
    }

    public void Load(BoardSaveData saveData) {
        categories = ISaveSerialization<CategorySaveData>.ConvertListFromSave<CategoryData>(saveData.categories, typeof(CategoryData)); ;
    }
}

[Serializable]
public struct BoardSaveData {
    public List<CategorySaveData> categories;
}
