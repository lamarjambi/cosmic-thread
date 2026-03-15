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

    public void CloseEvidence()
    {
        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        if (evidenceImage != null)
            evidenceImage.rectTransform.localScale = Vector3.one; // reset for next open
        evidencePanel?.SetActive(false);
    }
}