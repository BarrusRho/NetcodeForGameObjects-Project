using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace NetcodeForGameObjects.Network
{
    public class NetworkClient : IDisposable
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

        public void Dispose()
        {
            if (_networkManager != null)
            {
                _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
            }
        }
    }
}
