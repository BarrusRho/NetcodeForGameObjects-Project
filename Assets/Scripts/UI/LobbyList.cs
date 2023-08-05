using System;
using System.Collections;
using System.Collections.Generic;
using NetcodeForGameObjects.Network;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetcodeForGameObjects.UI
{
    public class LobbyList : MonoBehaviour
    {
        [SerializeField] private LobbyItem _lobbyItemPrefab;
        [SerializeField] private Transform _lobbyItemParent;

        private bool _isJoining;
        private bool _isRefreshing;

        private void OnEnable()
        {
            RefreshLobbyList();
        }

        public async void RefreshLobbyList()
        {
            if (_isRefreshing)
            {
                return;
            }

            _isRefreshing = true;

            try
            {
                var options = new QueryLobbiesOptions();
                options.Count = 25;

                options.Filters = new List<QueryFilter>()
                {
                    new QueryFilter(field: QueryFilter.FieldOptions.AvailableSlots, op: QueryFilter.OpOptions.GT,
                        value: "0"),
                    new QueryFilter(field: QueryFilter.FieldOptions.IsLocked, op: QueryFilter.OpOptions.EQ, value: "0")
                };

                var lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

                foreach (Transform child in _lobbyItemParent )
                {
                    Destroy(child.gameObject);
                }

                foreach (var lobby in lobbies.Results)
                {
                    var lobbyItem = Instantiate(_lobbyItemPrefab, _lobbyItemParent);
                    lobbyItem.InitialiseLobby(this, lobby);
                }
            }
            catch (LobbyServiceException exception)
            {
                Debug.Log(exception);
            }
            
            _isRefreshing = false;
        }

        public async void JoinLobbyAsync(Lobby lobby)
        {
            if (_isJoining)
            {
                return;
            }

            _isJoining = true;

            try
            {
                var joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
                var joinCode = joiningLobby.Data["JoinCode"].Value;

                await ClientSingleton.Instance.ClientGameManager.StartClientAsync(joinCode);
            }
            catch (LobbyServiceException exception)
            {
                Debug.Log(exception);
            }

            _isJoining = false;
        }
    }
}