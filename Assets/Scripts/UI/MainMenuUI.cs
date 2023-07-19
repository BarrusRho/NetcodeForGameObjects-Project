using System.Collections;
using System.Collections.Generic;
using NetcodeForGameObjects.Network;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _joinCodeInputField;

    public async void StartHost()
    {
        await HostSingleton.Instance.HostGameManager.StartHostAsync();
    }

    public async void StartClient()
    {
        await ClientSingleton.Instance.ClientGameManager.StartClientAsync(_joinCodeInputField.text);
    }
}
