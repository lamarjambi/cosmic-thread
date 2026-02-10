using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EvidenceClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Evidence Display")]
    [SerializeField] private GameObject evidencePanel;
    [SerializeField] private Image evidenceImage;
    [SerializeField] private Sprite evidenceSprite;
    
    void Start()
    {
        if (evidencePanel != null)
            evidencePanel.SetActive(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on " + gameObject.name);
        ShowEvidence();

        // for tutorialManager.cs
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnCardClicked();
        }
    }
    
    private void ShowEvidence()
    {
        if (evidencePanel != null)
        {
            evidencePanel.SetActive(true);
            
            if (evidenceImage != null && evidenceSprite != null)
                evidenceImage.sprite = evidenceSprite;
        }
    }

    public void CloseEvidence()
    {
        
        if (evidencePanel != null)
            evidencePanel.SetActive(false);
    }
}