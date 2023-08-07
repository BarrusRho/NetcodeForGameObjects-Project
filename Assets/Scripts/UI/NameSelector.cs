using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace NetcodeForGameObjects.UI
{
    public class NameSelector : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameField;
        [SerializeField] private Button _connectButton;
        [SerializeField] private int _minNameLength = 1;
        [SerializeField] private int _maxNameLength = 12;

        public const string PlayerNameKey = "PlayerName";
        
        private void Start()
        {
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }
            
            _nameField.text = PlayerPrefs.GetString(PlayerNameKey, string.Empty);
            ValidateNameChange();
        }

        public void ValidateNameChange()
        {
            _connectButton.interactable =
                _nameField.text.Length >= _minNameLength && _nameField.text.Length <= _maxNameLength;
        }

        public void Connect()
        {
            PlayerPrefs.SetString(PlayerNameKey, _nameField.text);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
