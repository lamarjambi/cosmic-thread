using UnityEngine;
using UnityEngine.EventSystems;

// Attach this to a full-screen transparent Image BEHIND evidenceImage inside evidencePanel
public class EvidencePanelClose : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EvidenceClick activeEvidenceClick;

    public void SetSource(EvidenceClick source) => activeEvidenceClick = source;

    public void OnPointerClick(PointerEventData eventData)
    {
        activeEvidenceClick?.CloseEvidence();
    }
}