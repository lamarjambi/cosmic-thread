using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip _backgroundMusic;
    private AudioSource _audioSource;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persists across scenes

        _audioSource = GetComponent<AudioSource>();

        if (_backgroundMusic != null) {
            _audioSource.clip = _backgroundMusic;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
}