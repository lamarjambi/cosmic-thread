using UnityEngine;
using UnityEngine.EventSystems;

public class EvidencePanelClose : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EvidenceClick activeEvidenceClick;

    public void SetSource(EvidenceClick source) => activeEvidenceClick = source;

    public void OnPointerClick(PointerEventData eventData)
    {
        activeEvidenceClick?.CloseEvidence();
    }
}