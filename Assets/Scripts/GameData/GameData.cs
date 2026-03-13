using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameData : IByteSerialization {
    [SerializeField]
    readonly List<BoardData> boards = new List<BoardData>();
    
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

    public void Deserialize(Packet pPacket) {
        boards.Clear();
        boards.AddRange(pPacket.ReadList<BoardData>());
    }

    public void Serialize(Packet pPacket) {
        pPacket.WriteList(boards);
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
}
