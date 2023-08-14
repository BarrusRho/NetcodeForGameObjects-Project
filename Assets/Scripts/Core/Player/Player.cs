using Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Core
{
    public class Player : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        
        [Header("Settings")]
        [SerializeField] private int _ownerPriority = 15;
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                _virtualCamera.Priority = _ownerPriority;
            }
        }
    }
}

