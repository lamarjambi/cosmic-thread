using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvidenceFlipClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EvidenceClick evidenceClick;

    public void SetSource(EvidenceClick source) => evidenceClick = source;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        evidenceClick.FlipCard();
    }
}