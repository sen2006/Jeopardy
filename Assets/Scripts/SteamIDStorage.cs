using PurrNet;
using PurrNet.Transports;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class SteamIDStorage : NetworkBehaviour
{
    [SerializeField] NetworkManager networkMNG;
    public readonly Dictionary<PlayerID, ulong> playerSteamIDs = new();
    public readonly Dictionary<PlayerID, string> playerNames = new();
    public readonly Dictionary<PlayerID, Texture2D> playerAvatars = new();

    private void Start() {
        networkMNG.onClientConnectionState += OnConnectStartClient;
    }

    public void OnConnectStartClient(ConnectionState state) {
        if (state == ConnectionState.Connected) {
            CSteamID steamID = SteamUser.GetSteamID();
            ulong steamId = steamID.m_SteamID;
            string name = SteamFriends.GetPersonaName();

            Texture2D avatar = GetSteamAvatar(steamID);

            byte[] avatarBytes = avatar.EncodeToPNG();

            RegisterPlayerServerRpc(steamId, name, avatarBytes);
        }
    }

    Texture2D GetSteamAvatar(CSteamID steamID) {
        int avatarHandle = SteamFriends.GetLargeFriendAvatar(steamID);

        if (avatarHandle == -1)
            return null;

        uint width, height;
        SteamUtils.GetImageSize(avatarHandle, out width, out height);

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
        avatar.LoadImage(avatarBytes);

        playerAvatars[info.sender] = avatar;

        Debug.Log($"Player {info.sender.id} registered as {name}");
    }
}
