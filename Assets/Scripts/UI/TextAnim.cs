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
        
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        
        if (_textMeshPro != null)
        {
            _fullText = _textMeshPro.text;
            _textMeshPro.maxVisibleCharacters = 0;
            
            if (_startAutomatically)
            {
                StartTypewriter();
            }
        }
    }

    /// <summary>
    /// Start the typewriter effect with the current text in TextMeshPro
    /// </summary>
    public void StartTypewriter()
    {
        if (_textMeshPro == null)
        {
            return;
        }
        
        if (_isTyping)
        {
            return;
        }
        
        _fullText = _textMeshPro.text;
        _textMeshPro.maxVisibleCharacters = 0;
        
        if (_currentTypewriterCoroutine != null)
        {
            StopCoroutine(_currentTypewriterCoroutine);
        }
        
        _currentTypewriterCoroutine = StartCoroutine(TextVisible());
    }
    
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
    
    public bool IsTyping => _isTyping;

    private IEnumerator TextVisible() 
    {
        _isTyping = true;
        
        int totalVisibleCharacters = _fullText.Length;

        for (int i = 0; i <= totalVisibleCharacters; i++) 
        {
            _textMeshPro.maxVisibleCharacters = i;
            
            if (i > 0 && i <= totalVisibleCharacters && _playSoundOnEachCharacter && _audioSource != null && _typingSound != null)
            {
                char currentChar = _fullText[i - 1];
                
                if (!char.IsWhiteSpace(currentChar))
                {
                    PlayTypingSound();
                }
            }
            
            yield return new WaitForSeconds(_typingSpeed);
        }
        
        _isTyping = false;
        _currentTypewriterCoroutine = null;
    }
    
    private void PlayTypingSound()
    {
        if (_audioSource != null && _typingSound != null)
        {
            _audioSource.PlayOneShot(_typingSound, _soundVolume);
        }
    }
    
    public void SetTypingSpeed(float speed)
    {
        _typingSpeed = speed;
    }
    
    public void SetSoundVolume(float volume)
    {
        _soundVolume = Mathf.Clamp01(volume);
    }
}