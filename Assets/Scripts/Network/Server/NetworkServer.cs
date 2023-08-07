using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Network
{
    public class NetworkServer
    {
        private NetworkManager _networkManager;

        public NetworkServer(NetworkManager networkManager)
        {
            _networkManager = networkManager;
            _networkManager.ConnectionApprovalCallback += ApprovalCheck;
        }

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            var payload = System.Text.Encoding.UTF8.GetString(request.Payload);
            var userData = JsonUtility.FromJson<UserData>(payload);
            
            Debug.Log(userData.userName);

            response.Approved = true;
            response.CreatePlayerObject = true;
        }
    }
}