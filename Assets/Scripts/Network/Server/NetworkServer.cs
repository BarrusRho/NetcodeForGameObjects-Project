using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Network
{
    public class NetworkServer : IDisposable
    {
        private NetworkManager _networkManager;

        private Dictionary<ulong, string> _clientIdToAuth = new Dictionary<ulong, string>();
        private Dictionary<string, UserData> _authIdToUserData = new Dictionary<string, UserData>();

        public NetworkServer(NetworkManager networkManager)
        {
            _networkManager = networkManager;
            _networkManager.ConnectionApprovalCallback += ApprovalCheck;
            _networkManager.OnServerStarted += OnNetworkReady;
        }

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            var payload = System.Text.Encoding.UTF8.GetString(request.Payload);
            var userData = JsonUtility.FromJson<UserData>(payload);

            _clientIdToAuth[request.ClientNetworkId] = userData.userAuthId;
            _authIdToUserData[userData.userAuthId] = userData;

            response.Approved = true;
            response.CreatePlayerObject = true;
        }
        
        private void OnNetworkReady()
        {
            _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void OnClientDisconnect(ulong clientId)
        {
            if (_clientIdToAuth.TryGetValue(clientId, out string authId))
            {
                _clientIdToAuth.Remove(clientId);
                _authIdToUserData.Remove(authId);
            }
        }

        public void Dispose()
        {
            if (_networkManager != null)
            {
                _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
                _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
                _networkManager.OnServerStarted -= OnNetworkReady;
            }

            if (_networkManager.IsListening)
            {
                _networkManager.Shutdown();
            }
        }
    }
}