using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GavelControl : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject culpritPanel;
    
    void Start()
    {
        if (culpritPanel != null)
            culpritPanel.SetActive(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.popUpIndex != 8)
            return;

        Debug.Log("clicked on " + gameObject.name);
        ShowCulprits();

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.OnGavelClicked();
    }
    
    private void ShowCulprits()
    {
        if (culpritPanel != null)
            culpritPanel.SetActive(true);
    }

}
