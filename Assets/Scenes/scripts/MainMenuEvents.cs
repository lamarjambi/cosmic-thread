using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument _document;

    private Button _button;

    private Button _startButton;
    private Button _settingsButton;
    private Button _quitButton;

    private List<Button> _menuButtons = new List<Button>();

    private void Awake() {
        _document = GetComponent<UIDocument>();

        var root = _document.rootVisualElement;
        Debug.Log($"Root element found: {root != null}");

        _startButton = root.Q<Button>("StartButton");
        _settingsButton = root.Q<Button>("SettingsButton");
        _quitButton = root.Q<Button>("QuitButton");

        Debug.Log($"StartButton found: {_startButton != null}");
        Debug.Log($"SettingsButton found: {_settingsButton != null}");
        Debug.Log($"QuitButton found: {_quitButton != null}");

        _startButton?.RegisterCallback<ClickEvent>(OnStartButtonClick);
        _settingsButton?.RegisterCallback<ClickEvent>(OnSettingsButtonClick);
        _quitButton?.RegisterCallback<ClickEvent>(OnQuitButtonClick);
    }

    private void OnDisable() {
        _startButton?.UnregisterCallback<ClickEvent>(OnStartButtonClick);
        _settingsButton?.UnregisterCallback<ClickEvent>(OnSettingsButtonClick);
        _quitButton?.UnregisterCallback<ClickEvent>(OnQuitButtonClick);
    }

    private void OnStartButtonClick(ClickEvent evt) {
        Debug.Log("Loading Backstory Scene...");
        SceneManager.LoadScene("BackstoryScene");
    }

    private void OnSettingsButtonClick(ClickEvent evt) {
        Debug.Log("Loading Settings Scene...");
        SceneManager.LoadScene("SettingsScene");
    }

    private void OnQuitButtonClick(ClickEvent evt) {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
