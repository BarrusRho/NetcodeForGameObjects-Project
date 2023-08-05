using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetcodeForGameObjects.UI
{
    public class LobbyItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lobbyNameText;
        [SerializeField] private TMP_Text _lobbyPlayersText;

        private LobbyList _lobbyList;
        private Lobby _lobby;

        public void InitialiseLobby(LobbyList lobbyList ,Lobby lobby)
        {
            _lobbyList = lobbyList;
            _lobby = lobby;
            _lobbyNameText.text = lobby.Name;
            _lobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        }

        public void JoinLobby()
        {
            _lobbyList.JoinLobbyAsync(_lobby);
        }
    }
}