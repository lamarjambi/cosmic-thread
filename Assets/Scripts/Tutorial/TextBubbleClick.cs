using UnityEngine;
using UnityEngine.EventSystems;

public class TextBubbleClick : MonoBehaviour, IPointerClickHandler
{
    public bool wasClicked = false;

    [Header("Click Sound")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] [Range(0f, 1f)] private float _clickVolume = 1f;

    private void Awake()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false;
            }
        }

        VolumeSettings.Route(_audioSource);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickSound != null)
        {
            _audioSource.PlayOneShot(_clickSound, _clickVolume);
        }

        wasClicked = true;
    }
}
