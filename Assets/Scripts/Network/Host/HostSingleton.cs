using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NetcodeForGameObjects.Network
{
    public class HostSingleton : MonoBehaviour
    {
        private static HostSingleton _instance;
        public static HostSingleton Instance => _instance;

        private HostGameManager _hostGameManager;
        public HostGameManager HostGameManager => _hostGameManager;

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

        public void CreateHost()
        {
            _hostGameManager = new HostGameManager();
        }
    }
}
