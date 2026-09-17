using UnityEngine;
using TMPro;
using System.Collections;

public class ModeIndicator : MonoBehaviour
{
    public bool isThreadMode = false;
    public TMP_Text modeText;
    public float flashDuration = 0.3f;
    public int flashCount = 2;

    [Header("Mode Switch Sound")]
    [SerializeField] private AudioSource whooshAudioSource;
    [Tooltip("Played when the player toggles between Inspect and Thread mode.")]
    [SerializeField] private AudioClip whoosh;
    [SerializeField] [Range(0f, 1f)] private float whooshVolume = 1f;

    private void Awake()
    {
        if (whooshAudioSource == null)
        {
            whooshAudioSource = GetComponent<AudioSource>();

            if (whooshAudioSource == null)
            {
                whooshAudioSource = gameObject.AddComponent<AudioSource>();
                whooshAudioSource.playOnAwake = false;
            }
        }
    }

    private void Start()
    {
        UpdateModeText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isThreadMode = !isThreadMode;
            UpdateModeText();

            if (whoosh != null && whooshAudioSource != null)
                whooshAudioSource.PlayOneShot(whoosh, whooshVolume);

            StopCoroutine(nameof(FlashText)); 
            StartCoroutine(nameof(FlashText));
        }
    }

    private void UpdateModeText()
    {
        modeText.text = isThreadMode ? "Thread Mode" : "Inspect Mode";
    }

    private IEnumerator FlashText()
    {
        Color original = modeText.color;

        for (int i = 0; i < flashCount; i++)
        {
            
            yield return LerpAlpha(original.a, 0f, flashDuration / 2);
            
            yield return LerpAlpha(0f, original.a, flashDuration / 2);
        }

        modeText.color = original; 
    }

    private IEnumerator LerpAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = modeText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / duration);
            modeText.color = c;
            yield return null;
        }

        c.a = to;
        modeText.color = c;
    }
}