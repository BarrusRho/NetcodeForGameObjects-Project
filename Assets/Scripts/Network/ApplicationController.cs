using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NetcodeForGameObjects.Network
{
    public class ApplicationController : MonoBehaviour
    {
        [SerializeField] private ClientSingleton _clientSingleton;
        [SerializeField] private HostSingleton _hostSingleton;

        private async void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
        }

        private async Task LaunchInMode(bool isDedicatedServer)
        {
            if (isDedicatedServer)
            {

            }
            else
            {
                var hostSingleton = Instantiate(_hostSingleton);
                hostSingleton.CreateHost();

                var clientSingleton = Instantiate(_clientSingleton);
                var isAuthenticated = await clientSingleton.CreateClient();

                if (isAuthenticated)
                {
                    clientSingleton.ClientGameManager.GoToMainMenu();
                }
            }
        }
    }
}
