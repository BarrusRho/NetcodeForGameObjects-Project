using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using System;
using System.Text;
using NetcodeForGameObjects.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;

namespace NetcodeForGameObjects.Network
{
    public class ClientGameManager : IDisposable
    {
        private JoinAllocation _joinAllocation;
        private NetworkClient _networkClient;

        private const string _mainMenuSceneName = "MainMenu";

        public async Task<bool> InitAsync()
        {
            await UnityServices.InitializeAsync();

            _networkClient = new NetworkClient(NetworkManager.Singleton);

            var authenticationState = await AuthenticationManager.AuthenticateAsync();
            if (authenticationState == AuthenticationState.Authenticated)
            {
                return true;
            }

            return false;
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        public async Task StartClientAsync(string joinCode)
        {
            try
            {
                _joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return;
            }

            var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            var relayServerData = new RelayServerData(_joinAllocation, "dtls");
            unityTransport.SetRelayServerData(relayServerData);

            var userData = new UserData()
            {
                userName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name"),
                userAuthId = AuthenticationService.Instance.PlayerId
            };

            var payload = JsonUtility.ToJson(userData);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

            NetworkManager.Singleton.StartClient();
        }

        public void Dispose()
        {
            _networkClient?.Dispose();
        }
    }
}
