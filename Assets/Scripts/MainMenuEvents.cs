using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument _document;

    private Button _startButton;
    private Button _settingsButton;
    private Button _quitButton;

    private List<Button> _menuButtons = new List<Button>();

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.Play();
    }

    private void Awake() {
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;

        _startButton = root.Q<Button>("StartButton");
        _settingsButton = root.Q<Button>("SettingsButton");
        _quitButton = root.Q<Button>("QuitButton");

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
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnSettingsButtonClick(ClickEvent evt) {
        SceneManager.LoadScene("SettingsScene");
    }

    private void OnQuitButtonClick(ClickEvent evt) {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}