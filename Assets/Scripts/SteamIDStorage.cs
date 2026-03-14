using PurrNet;
using PurrNet.Transports;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIDStorage : NetworkBehaviour {
    [SerializeField] NetworkManager networkMNG;
    public static readonly Dictionary<PlayerID, ulong> playerSteamIDs = new();
    public static readonly Dictionary<PlayerID, string> playerNames = new();
    public static readonly Dictionary<PlayerID, Texture2D> playerAvatars = new();

    private bool pendingRegister = false;
    private ulong pendingSteamId;
    private string pendingName;
    private byte[] pendingAvatar;

    private void Start() {
        networkMNG.onClientConnectionState += OnConnectStartClient;
    }

    private void OnDestroy() {
        // Unsubscribe to avoid memory leaks
        networkMNG.onClientConnectionState -= OnConnectStartClient;
    }

    public void OnConnectStartClient(ConnectionState state) {
        if (state == ConnectionState.Connected) {
            CSteamID steamID = SteamUser.GetSteamID();
            pendingSteamId = steamID.m_SteamID;
            pendingName = SteamFriends.GetPersonaName();

            Texture2D avatar = GetSteamAvatar(steamID);
            pendingAvatar = avatar != null ? avatar.EncodeToPNG() : new byte[0];

            pendingRegister = true;
        }
    }

    private void Update() {
        if (pendingRegister && isSpawned) {
            RegisterPlayerServerRpc(pendingSteamId, pendingName, pendingAvatar);
            pendingRegister = false;
        }
    }

    private IEnumerator SendRegisterRPCWhenSpawned() {
        // Wait until this NetworkBehaviour is spawned
        yield return new WaitUntil(() => isSpawned);

        CSteamID steamID = SteamUser.GetSteamID();
        ulong steamId = steamID.m_SteamID;
        string name = SteamFriends.GetPersonaName();

        Texture2D avatar = GetSteamAvatar(steamID);
        byte[] avatarBytes = avatar != null ? avatar.EncodeToPNG() : new byte[0];

        RegisterPlayerServerRpc(steamId, name, avatarBytes);
    }

    private Texture2D GetSteamAvatar(CSteamID steamID) {
        int avatarHandle = SteamFriends.GetLargeFriendAvatar(steamID);
        if (avatarHandle == -1)
            return null;

        SteamUtils.GetImageSize(avatarHandle, out uint width, out uint height);

        byte[] image = new byte[width * height * 4];
        SteamUtils.GetImageRGBA(avatarHandle, image, (int)(width * height * 4));

        Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(image);
        texture.Apply();

        return texture;
    }

    [ServerRpc]
    private void RegisterPlayerServerRpc(ulong steamId, string name, byte[] avatarBytes, RPCInfo info = default) {
        playerSteamIDs[info.sender] = steamId;
        playerNames[info.sender] = name;

        Texture2D avatar = new Texture2D(2, 2);
        if (avatarBytes.Length > 0)
            avatar.LoadImage(avatarBytes);

        playerAvatars[info.sender] = avatar;

        Debug.Log($"Player {info.sender.id} registered as {name}");
    }
}