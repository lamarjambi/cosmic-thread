using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EvidenceClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject evidencePanel;
    [SerializeField] private Image evidenceImage; // placeholders for now
    [SerializeField] private Sprite evidenceSprite;
    [SerializeField] ModeIndicator modeIndicator; 
    
    void Start()
    {
        if (evidencePanel != null)
            evidencePanel.SetActive(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked on " + gameObject.name);
        ShowEvidence();

        // for tutorialManager.cs
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnCardClicked();
        }
    }
    
    private void ShowEvidence()
    {
        // :func: show evidence only if we're in inspect mode
        if ((evidencePanel != null) && (!modeIndicator.isThreadMode))
        {
            evidencePanel.SetActive(true);
            
            if (evidenceImage != null && evidenceSprite != null)
                evidenceImage.sprite = evidenceSprite;
        }
    }

    public void CloseEvidence()
    {
        evidencePanel?.SetActive(false); // week 3 - ternary operator
    }
}