using System.Collections;
using System.Collections.Generic;
using NetcodeForGameObjects.Network;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public async void StartHost()
    {
        await HostSingleton.Instance.HostGameManager.StartHostAsync();
    }
}
