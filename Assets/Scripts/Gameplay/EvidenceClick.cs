using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EvidenceClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject evidencePanel;
    [SerializeField] private Image evidenceImage;
    [SerializeField] private Sprite evidenceSprite;
    [SerializeField] private ModeIndicator modeIndicator;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomDuration = 0.35f;
    [SerializeField] private AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float zoomOutDuration = 0.3f;
    [Tooltip("Hide this card on the board while it is being inspected, as if it were picked up.")]
    [SerializeField] private bool hideCardWhileZoomed = true;

    private Coroutine _zoomCoroutine;
    private Vector2 _panelAnchoredPos;
    private bool _panelPosCached;
    private bool _isClosing;
    private CanvasGroup _panelGroup;
    private CanvasGroup _cardGroup;
    private float _cardAlpha = 1f;

    [Header("Pickup Sound")]
    [SerializeField] private AudioSource pickupAudioSource;
    [Tooltip("Played when this item is lifted off the board. Paper vs plastic, per item.")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] [Range(0f, 1f)] private float pickupVolume = 1f;

    [Header("Flip Settings")]
    [SerializeField] private Sprite cardBackSprite; // the card's back face
    [SerializeField] private float flipDuration = 0.4f;

    private bool isFlipped = false;

    private void Awake()
    {
        if (pickupAudioSource == null)
        {
            pickupAudioSource = GetComponent<AudioSource>();

            if (pickupAudioSource == null)
            {
                pickupAudioSource = gameObject.AddComponent<AudioSource>();
                pickupAudioSource.playOnAwake = false;
            }
        }

        VolumeSettings.Route(pickupAudioSource);
    }

    void Start()
    {
        CachePanelPose();

        if (evidencePanel != null)
            evidencePanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked on " + gameObject.name);
        ShowEvidence();

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.OnCardClicked();
    }

    private void ShowEvidence()
    {
        evidencePanel.GetComponentInChildren<EvidencePanelClose>()?.SetSource(this);
        evidencePanel.GetComponentInChildren<EvidenceFlipClick>()?.SetSource(this);

        if (evidencePanel == null || modeIndicator.isThreadMode) return;

        if (evidenceImage != null && evidenceSprite != null)
            evidenceImage.sprite = evidenceSprite;

        isFlipped = false;
        _isClosing = false;

        CachePanelPose();
        if (_panelPosCached && evidenceImage != null)
            evidenceImage.rectTransform.anchoredPosition = _panelAnchoredPos;

        if (_panelGroup != null) _panelGroup.alpha = 1f;

        if (pickupSound != null && pickupAudioSource != null)
            pickupAudioSource.PlayOneShot(pickupSound, pickupVolume);

        SetCardOnBoardVisible(false);
        evidencePanel.SetActive(true);

        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        _zoomCoroutine = StartCoroutine(ZoomIn());
    }

    private IEnumerator ZoomIn()
    {
        RectTransform rt = evidenceImage.rectTransform;
        rt.localScale = Vector3.zero;

        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = zoomCurve.Evaluate(elapsed / zoomDuration);
            rt.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        rt.localScale = Vector3.one;
    }

    public void FlipCard()
    {
        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        _zoomCoroutine = StartCoroutine(FlipCoroutine());
    }

    private IEnumerator FlipCoroutine()
    {
        RectTransform rt = evidenceImage.rectTransform;
        float half = flipDuration / 2f;

        // Phase 1: squish to flat
        float elapsed = 0f;
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / half;
            rt.localScale = new Vector3(1f - t, 1f, 1f);
            yield return null;
        }

        // Swap sprite at the midpoint
        isFlipped = !isFlipped;
        evidenceImage.sprite = isFlipped ? cardBackSprite : evidenceSprite;

        // Phase 2: unsquish back
        elapsed = 0f;
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / half;
            rt.localScale = new Vector3(t, 1f, 1f);
            yield return null;
        }

        rt.localScale = Vector3.one;
    }

    public void CloseEvidence()
    {
        if (_isClosing) return;

        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);

        if (evidenceImage == null || evidencePanel == null || !evidencePanel.activeInHierarchy)
        {
            FinishClose();
            return;
        }

        _isClosing = true;
        _zoomCoroutine = StartCoroutine(ZoomOut());
    }

    private IEnumerator ZoomOut()
    {
        RectTransform rt = evidenceImage.rectTransform;

        Vector3 startScale = rt.localScale;
        Vector2 startPos = rt.anchoredPosition;
        Vector2 endPos = startPos + GetOffsetToCard(rt);
        float startAlpha = _panelGroup != null ? _panelGroup.alpha : 1f;

        float elapsed = 0f;
        while (elapsed < zoomOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = zoomCurve.Evaluate(Mathf.Clamp01(elapsed / zoomOutDuration));

            rt.localScale = Vector3.LerpUnclamped(startScale, Vector3.zero, t);
            rt.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, t);

            if (_panelGroup != null)
                _panelGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);

            yield return null;
        }

        _zoomCoroutine = null;
        FinishClose();
    }

    private void FinishClose()
    {
        if (evidenceImage != null)
        {
            RectTransform rt = evidenceImage.rectTransform;
            rt.localScale = Vector3.one;
            if (_panelPosCached) rt.anchoredPosition = _panelAnchoredPos;
        }

        if (_panelGroup != null) _panelGroup.alpha = 1f;

        if (evidencePanel != null) evidencePanel.SetActive(false);
        SetCardOnBoardVisible(true);
        _isClosing = false;
    }

    private void SetCardOnBoardVisible(bool visible)
    {
        if (!hideCardWhileZoomed) return;

        if (_cardGroup == null)
        {
            _cardGroup = GetComponent<CanvasGroup>();
            if (_cardGroup == null && transform is RectTransform)
                _cardGroup = gameObject.AddComponent<CanvasGroup>();

            if (_cardGroup != null) _cardAlpha = _cardGroup.alpha;
        }

        if (_cardGroup != null)
        {
            _cardGroup.alpha = visible ? _cardAlpha : 0f;
            _cardGroup.blocksRaycasts = visible;
            _cardGroup.interactable = visible;
            return;
        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
            r.enabled = visible;
    }

    private void CachePanelPose()
    {
        if (_panelGroup == null && evidencePanel != null)
            _panelGroup = evidencePanel.GetComponent<CanvasGroup>();

        if (_panelPosCached || evidenceImage == null) return;

        _panelAnchoredPos = evidenceImage.rectTransform.anchoredPosition;
        _panelPosCached = true;
    }

    private Vector2 GetOffsetToCard(RectTransform rt)
    {
        RectTransform parent = rt.parent as RectTransform;
        if (parent == null) return Vector2.zero;

        Camera uiCamera = CameraFor(evidenceImage.canvas);
        Camera cardCamera = CameraFor(GetComponentInParent<Canvas>());

        Vector2 cardScreen = RectTransformUtility.WorldToScreenPoint(cardCamera, transform.position);
        Vector2 imageScreen = RectTransformUtility.WorldToScreenPoint(uiCamera, rt.position);

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, cardScreen, uiCamera, out Vector2 cardLocal) ||
            !RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, imageScreen, uiCamera, out Vector2 imageLocal))
            return Vector2.zero;

        return cardLocal - imageLocal;
    }
    private static Camera CameraFor(Canvas canvas)
    {
        if (canvas == null) return null;

        Canvas root = canvas.rootCanvas != null ? canvas.rootCanvas : canvas;
        return root.renderMode == RenderMode.ScreenSpaceOverlay ? null : root.worldCamera;
    }
}