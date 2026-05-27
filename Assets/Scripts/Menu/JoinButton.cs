using PurrLobby;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class JoinButton : MonoBehaviour {
    [SerializeField] private TMP_InputField roomIdInput;
    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private UnityEvent onStartJoin;

    public void JoinRoom() {
        if (string.IsNullOrEmpty(roomIdInput.text)) {
            Debug.LogWarning($"Can't start join, room ID is empty.");
            return;
        }

        string roomId = roomIdInput.text;

        ulong longId = CodeConverter.toUlong(roomId);

        onStartJoin?.Invoke();
        lobbyManager.JoinLobby(longId.ToString());
    }
}