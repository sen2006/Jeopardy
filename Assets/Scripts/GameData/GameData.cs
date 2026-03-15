using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData : ISaveSerialization<GameSaveData> {
    [SerializeField]
    List<BoardData> boards = new List<BoardData>();
    
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

    internal void loadBoard(GameObject renderParent, int index) {
        boards[index].loadToScene(renderParent);
    }

    internal string GetGameName() {
        return "Game Name";
    }

    internal static int GetStartingCash() {
        return 0;
    }

    public GameSaveData Save() {
        GameSaveData saveData = new GameSaveData();

        List<BoardSaveData> boardSave = ISaveSerialization<BoardSaveData>.ConvertListToSave(boards);

        saveData.boards = boardSave;
        return saveData;
    }

    public void Load(GameSaveData saveData) {
        boards = ISaveSerialization<BoardSaveData>.ConvertListFromSave<BoardData>(saveData.boards, typeof(BoardData));
    }
}

[Serializable]
public struct GameSaveData {
    public List<BoardSaveData> boards;
}
