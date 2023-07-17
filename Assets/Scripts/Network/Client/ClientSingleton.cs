using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NetcodeForGameObjects.Network
{
    public class ClientSingleton : MonoBehaviour
    {
        private static ClientSingleton _instance;
        public static ClientSingleton Instance => _instance;

        private ClientGameManager _clientGameManager;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public async Task CreateClient()
        {
            _clientGameManager = new ClientGameManager();
            await _clientGameManager.InitAsync();
        }
    }
}
