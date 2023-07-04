using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodeForGameObjects.Testing
{
    public class JoinServerTest : MonoBehaviour
    {
        public void Join()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
