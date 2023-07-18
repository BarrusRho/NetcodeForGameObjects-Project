using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using System;
using UnityEngine.SceneManagement;

namespace NetcodeForGameObjects.Network
{
    public class ClientGameManager
    {
        private const string _mainMenuSceneName = "MainMenu";

        public async Task<bool> InitAsync()
        {
            await UnityServices.InitializeAsync();

            var authenticationState = await AuthenticationManager.AuthenticateAsync();
            if(authenticationState == AuthenticationState.Authenticated)
            {
                return true;
            }

            return false;
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }
    }
}
