using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData : ISaveSerialization<GameSaveData> {
    [SerializeField] string name = "board" ;
    [SerializeField] List<BoardData> boards = new List<BoardData>();
    
    public BoardData AddNewBoard() {
        BoardData board = new BoardData();
        boards.Add(board);
        return board;
    }

    public BoardData InsertNewBoard(int index) {
        BoardData board = new BoardData();
        boards.Insert(index, board);
        return board;
    }

    public BoardData GetBoard(int index) {
        return boards[index];
    }

    public BoardData[] GetAllBoards() {
        return boards.ToArray();
    }

    public int GetBoardCount() { 
        return boards.Count; 
    }

    public string GetGameName() {
        if (name == null)
            return "unnamed";
        return name;
    }

    public static int GetStartingCash() {
        return 0;
    }

    public GameSaveData Save() {
        GameSaveData saveData = new GameSaveData();

        saveData.name = name;

        List<BoardSaveData> boardSave = ISaveSerialization<BoardSaveData>.ConvertListToSave(boards);

        saveData.boards = boardSave;
        return saveData;
    }

    public void Load(GameSaveData saveData) {
        name = saveData.name;
        boards = ISaveSerialization<BoardSaveData>.ConvertListFromSave<BoardData>(saveData.boards, typeof(BoardData));
    }

    internal void DeleteBoard(int selectedBoardIndex) {
        if (boards.Count >0)
            boards.RemoveAt(selectedBoardIndex);
    }

    internal void setName(string newName) {
        name = newName;
    }
}

[Serializable]
public struct GameSaveData {
    public string name;
    public List<BoardSaveData> boards;
}
