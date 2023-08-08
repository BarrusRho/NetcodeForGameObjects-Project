using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetcodeForGameObjects.Network
{
    public class NetworkClient
    {
        private NetworkManager _networkManager;

        private const string MainMenuSceneName = "MainMenu";

        public NetworkClient(NetworkManager networkManager)
        {
            _networkManager = networkManager;
            _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void OnClientDisconnect(ulong clientId)
        {
            if (clientId != 0 && clientId != _networkManager.LocalClientId)
            {
                return;
            }

            if (SceneManager.GetActiveScene().name != MainMenuSceneName)
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }

            if (_networkManager.IsConnectedClient)
            {
                _networkManager.Shutdown();
            }
        }
    }
}
