using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _typingSound;
    
    [Header("Typewriter Settings")]
    [SerializeField] private float _typingSpeed = 0.08f; // time between characters in seconds
    [SerializeField] private bool _playSoundOnEachCharacter = true;
    [SerializeField] private bool _startAutomatically = true;
    [SerializeField] [Range(0f, 1f)] private float _soundVolume = 1f; // volume 
    
    public string[] stringArray;
    
    private Coroutine _currentTypewriterCoroutine;
    private string _fullText;
    private bool _isTyping = false;

    void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        // Configure AudioSource for optimal typewriter sound playback
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        
        // Set up the full text from the TextMeshPro component
        if (_textMeshPro != null)
        {
            _fullText = _textMeshPro.text;
            // Start with text completely hidden
            _textMeshPro.maxVisibleCharacters = 0;
            
            // Start the typewriter effect automatically if enabled
            if (_startAutomatically)
            {
                StartTypewriter();
            }
        }
        else
        {
            Debug.LogError("TextAnim: TextMeshProUGUI component not found! Make sure the script is on the same GameObject as the Text (TMP) component.");
        }
    }

    /// <summary>
    /// Start the typewriter effect with the current text in TextMeshPro
    /// </summary>
    public void StartTypewriter()
    {
        if (_textMeshPro == null)
        {
            Debug.LogError("TextAnim: TextMeshProUGUI component is null!");
            return;
        }
        
        if (_isTyping)
        {
            Debug.Log("TextAnim: Already typing, skipping start request.");
            return;
        }
        
        _fullText = _textMeshPro.text;
        _textMeshPro.maxVisibleCharacters = 0;
        
        Debug.Log($"TextAnim: Starting typewriter effect with text: '{_fullText}' (Length: {_fullText.Length})");
        
        if (_currentTypewriterCoroutine != null)
        {
            StopCoroutine(_currentTypewriterCoroutine);
        }
        
        _currentTypewriterCoroutine = StartCoroutine(TextVisible());
    }
    
    /// <summary>
    /// Start the typewriter effect with custom text
    /// </summary>
    /// <param name="textToType">The text to display with typewriter effect</param>
    public void StartTypewriter(string textToType)
    {
        if (_textMeshPro == null || _isTyping) return;
        
        _textMeshPro.text = textToType;
        _fullText = textToType;
        _textMeshPro.maxVisibleCharacters = 0;
        
        if (_currentTypewriterCoroutine != null)
        {
            StopCoroutine(_currentTypewriterCoroutine);
        }
        
        _currentTypewriterCoroutine = StartCoroutine(TextVisible());
    }
    
    /// <summary>
    /// Stop the current typewriter effect and show all text immediately
    /// </summary>
    public void SkipTypewriter()
    {
        if (_currentTypewriterCoroutine != null)
        {
            StopCoroutine(_currentTypewriterCoroutine);
            _currentTypewriterCoroutine = null;
        }
        
        if (_textMeshPro != null)
        {
            _textMeshPro.maxVisibleCharacters = _fullText.Length;
        }
        
        _isTyping = false;
    }
    
    /// <summary>
    /// Check if the typewriter effect is currently running
    /// </summary>
    public bool IsTyping => _isTyping;

    private IEnumerator TextVisible() 
    {
        _isTyping = true;
        
        int totalVisibleCharacters = _fullText.Length;

        for (int i = 0; i <= totalVisibleCharacters; i++) 
        {
            _textMeshPro.maxVisibleCharacters = i;
            
            // Play typing sound for each character (except spaces and at the start)
            if (i > 0 && i <= totalVisibleCharacters && _playSoundOnEachCharacter && _audioSource != null && _typingSound != null)
            {
                char currentChar = _fullText[i - 1];
                
                // Only play sound for visible characters (not spaces, newlines, tabs)
                if (!char.IsWhiteSpace(currentChar))
                {
                    PlayTypingSound();
                }
            }
            
            // Wait before showing the next character
            yield return new WaitForSeconds(_typingSpeed);
        }
        
        _isTyping = false;
        _currentTypewriterCoroutine = null;
    }
    
    /// <summary>
    /// Play the typing sound effect
    /// </summary>
    private void PlayTypingSound()
    {
        if (_audioSource != null && _typingSound != null)
        {
            // Use PlayOneShot to allow multiple sounds to overlap if needed
            _audioSource.PlayOneShot(_typingSound, _soundVolume);
        }
    }
    
    /// <summary>
    /// Set the typing speed (delay between characters)
    /// </summary>
    /// <param name="speed">Delay in seconds between each character</param>
    public void SetTypingSpeed(float speed)
    {
        _typingSpeed = speed;
    }
    
    /// <summary>
    /// Set the volume of the typing sound
    /// </summary>
    /// <param name="volume">Volume between 0 and 1</param>
    public void SetSoundVolume(float volume)
    {
        _soundVolume = Mathf.Clamp01(volume);
    }
}