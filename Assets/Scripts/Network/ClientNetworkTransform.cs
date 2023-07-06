using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

namespace NetcodeForGameObjects.Network
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override void Update()
        {
            CanCommitToTransform = IsOwner;
            base.Update();

            if (NetworkManager != null)
            {
                if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
                {
                    if (CanCommitToTransform)
                    {
                        TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                    }
                }

            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            CanCommitToTransform = IsOwner;
        }

        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
