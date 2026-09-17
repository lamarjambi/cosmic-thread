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
    [SerializeField] private float _typingSpeed = 0.08f; // time between characters in seconds (only used when there is no voice clip)
    [SerializeField] private bool _playSoundOnEachCharacter = true;
    [SerializeField] private bool _startAutomatically = true;
    [SerializeField] [Range(0f, 1f)] private float _soundVolume = 1f; // volume

    [Header("Voice Over")]
    [SerializeField] private AudioClip _voiceClip; // line the character speaks while this text types out
    [SerializeField] private bool _matchTypingToVoice = true; // stretch the typing so it ends with the clip
    [SerializeField] [Range(0f, 1f)] private float _voiceVolume = 1f;
    [SerializeField] private float _voiceTailPadding = 0f; // finish the text this many seconds before the clip ends

    public string[] stringArray;

    // one source shared by every TextAnim so two lines can never overlap
    private static AudioSource _voiceSource;
    private static TextAnim _voiceOwner;

    private Coroutine _currentTypewriterCoroutine;
    private string _fullText;
    private bool _isTyping = false;
    private bool _isInitialised = false;

    void Start()
    {
        Initialise();

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

    // Start() never runs while the pop up is inactive, so whatever the
    // typewriter needs is set up here and called from both places
    private void Initialise()
    {
        if (_isInitialised) return;
        _isInitialised = true;

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

        Initialise();

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

        Initialise();

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
            _textMeshPro.ForceMeshUpdate();
            _textMeshPro.maxVisibleCharacters = _textMeshPro.textInfo.characterCount;
        }

        StopVoice(); // the line gets cut off along with the text
        _isTyping = false;
    }

    public bool IsTyping => _isTyping;

    public void SetVoiceClip(AudioClip clip)
    {
        _voiceClip = clip;
    }

    private IEnumerator TextVisible()
    {
        _isTyping = true;

        _textMeshPro.ForceMeshUpdate(); // so characterCount ignores rich text tags
        TMP_TextInfo textInfo = _textMeshPro.textInfo;
        int totalVisibleCharacters = textInfo.characterCount;

        float voiceDuration = 0f;

        if (_voiceClip != null)
        {
            PlayVoice();

            if (_matchTypingToVoice)
            {
                voiceDuration = Mathf.Max(0.01f, _voiceClip.length - _voiceTailPadding);
            }
        }

        if (voiceDuration > 0f && totalVisibleCharacters > 0)
        {
            // drive the typing off the clip itself, so the last character lands
            // on the last word instead of drifting away from it
            float elapsed = 0f;
            int visibleCharacters = 0;

            while (visibleCharacters < totalVisibleCharacters)
            {
                elapsed = IsVoicePlaying() ? _voiceSource.time : elapsed + Time.unscaledDeltaTime;

                int target = Mathf.Clamp(
                    Mathf.FloorToInt(totalVisibleCharacters * (elapsed / voiceDuration)),
                    0,
                    totalVisibleCharacters);

                while (visibleCharacters < target)
                {
                    visibleCharacters++;

                    if (_playSoundOnEachCharacter && !char.IsWhiteSpace(textInfo.characterInfo[visibleCharacters - 1].character))
                    {
                        PlayTypingSound();
                    }
                }

                _textMeshPro.maxVisibleCharacters = visibleCharacters;

                yield return null;
            }
        }
        else
        {
            for (int i = 0; i <= totalVisibleCharacters; i++)
            {
                _textMeshPro.maxVisibleCharacters = i;

                if (i > 0 && _playSoundOnEachCharacter && !char.IsWhiteSpace(textInfo.characterInfo[i - 1].character))
                {
                    PlayTypingSound();
                }

                yield return new WaitForSeconds(_typingSpeed);
            }
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

    private void PlayVoice()
    {
        if (_voiceSource == null)
        {
            // lives on its own object so deactivating a pop up does not cut the
            // audio off behind our back (the scene load still cleans it up)
            GameObject host = new GameObject("VoiceOverSource");
            _voiceSource = host.AddComponent<AudioSource>();
            _voiceSource.playOnAwake = false;
            _voiceSource.loop = false;
        }

        _voiceSource.Stop(); // whatever was talking before stops right here
        _voiceSource.clip = _voiceClip;
        _voiceSource.volume = _voiceVolume;
        _voiceOwner = this;
        _voiceSource.Play();
    }

    private bool IsVoicePlaying()
    {
        return _voiceSource != null && _voiceOwner == this && _voiceSource.isPlaying;
    }

    /// <summary>
    /// Stops this line's voice over, leaves any newer line alone
    /// </summary>
    public void StopVoice()
    {
        if (_voiceSource != null && _voiceOwner == this)
        {
            _voiceSource.Stop();
            _voiceSource.clip = null;
            _voiceOwner = null;
        }
    }

    void OnDisable()
    {
        StopVoice();
        _currentTypewriterCoroutine = null;
        _isTyping = false;
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
