using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetcodeForGameObjects.Network
{
    public class HostGameManager
    {
        private Allocation _allocation;
        private string _joinCode;
        private string _lobbyId;
        private const int _maxConnections = 20;

        private const string _gameSceneName = "Main";

        public async Task StartHostAsync()
        {
            try
            {
                _allocation = await Relay.Instance.CreateAllocationAsync(_maxConnections);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return;
            }

            try
            {
                _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);
                Debug.Log($"Join code: {_joinCode}");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return;
            }

            var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            var relayServerData = new RelayServerData(_allocation, "dtls");
            unityTransport.SetRelayServerData(relayServerData);

            try
            {
                var lobbyOptions = new CreateLobbyOptions();
                lobbyOptions.IsPrivate = false;
                lobbyOptions.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Member, value: _joinCode)
                    }
                };

                var lobby = await Lobbies.Instance.CreateLobbyAsync("My Lobby", _maxConnections, lobbyOptions);
                _lobbyId = lobby.Id;

                HostSingleton.Instance.StartCoroutine(HeartbeatLobbyRoutine(15));
            }
            catch (LobbyServiceException exception)
            {
                Debug.Log(exception);
                return;
            }

            NetworkManager.Singleton.StartHost();

            NetworkManager.Singleton.SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
        }

        private IEnumerator HeartbeatLobbyRoutine(float waitTimeSeconds)
        {
            var delay = new WaitForSecondsRealtime(waitTimeSeconds);

            while (true)
            {
                Lobbies.Instance.SendHeartbeatPingAsync(_lobbyId);
                yield return delay;
            }
        }
    }
}