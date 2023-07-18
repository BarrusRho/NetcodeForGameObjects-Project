using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace NetcodeForGameObjects.Network
{
    public static class AuthenticationManager
    {
        public static AuthenticationState AuthenticationState { get; private set; } = AuthenticationState.Unauthenticated;

        public static async Task<AuthenticationState> AuthenticateAsync(int maxAuthenticationAttempts = 5)
        {
            if (AuthenticationState == AuthenticationState.Authenticated)
            {
                return AuthenticationState;
            }

            if(AuthenticationState == AuthenticationState.Authenticating)
            {
                Debug.Log("Authentication is already in progress. Waiting for it to complete.");
                await CheckCurrentAuthenticationState();
                return AuthenticationState;
            }

            await SignInAnonymouslyAsync(maxAuthenticationAttempts);

            return AuthenticationState;
        }

        private static async Task SignInAnonymouslyAsync(int maxAuthenticationAttempts)
        {
            AuthenticationState = AuthenticationState.Authenticating;

            var authenticationAttempts = 0;
            while (AuthenticationState == AuthenticationState.Authenticating && authenticationAttempts < maxAuthenticationAttempts)
            {
                try
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

                    if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                    {
                        AuthenticationState = AuthenticationState.Authenticated;
                        break;
                    }
                }
                catch (AuthenticationException authenticationException)
                {
                    Debug.LogError($"AuthenticationException: {authenticationException.Message}");
                    AuthenticationState = AuthenticationState.Error;
                }
                catch (RequestFailedException requestFailedException)
                {
                    Debug.LogError($"RequestFailedException: {requestFailedException.Message}");
                    AuthenticationState = AuthenticationState.Error;
                }

                authenticationAttempts++;
                await Task.Delay(1000);
            }

            if (AuthenticationState != AuthenticationState.Authenticated)
            {
                Debug.LogError($"Player was not signed in successfully after {authenticationAttempts} times.");
                AuthenticationState = AuthenticationState.TimeOut;
            }
        }

        private static async Task<AuthenticationState> CheckCurrentAuthenticationState()
        {
            while(AuthenticationState == AuthenticationState.Authenticating || AuthenticationState == AuthenticationState.Unauthenticated)
            {
                await Task.Delay(200);
            }

            return AuthenticationState;
        }

    }

    public enum AuthenticationState
    {
        Unauthenticated,
        Authenticating,
        Authenticated,
        Error,
        TimeOut
    }
}
