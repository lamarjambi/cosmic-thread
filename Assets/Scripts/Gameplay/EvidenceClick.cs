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

    private Coroutine _zoomCoroutine;

    [Header("Flip Settings")]
    [SerializeField] private Sprite cardBackSprite; // the card's back face
    [SerializeField] private float flipDuration = 0.4f;

    private bool isFlipped = false;
    void Start()
    {
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
        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        if (evidenceImage != null)
            evidenceImage.rectTransform.localScale = Vector3.one; 
        evidencePanel?.SetActive(false);
    }
}