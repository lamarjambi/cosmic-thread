using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CulpritClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private CulpritPanel culpritPanel;
    
    void Start()
    {
        if (resultScreen != null)
            resultScreen.SetActive(false);  
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.popUpIndex != 12)
            return;

        Debug.Log("clicked on " + gameObject.name);
        culpritPanel.OnCulpritSelected(gameObject);

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.OnResultClicked();
    }
    
    private void ShowResult()
    {
        // :func: show evidence only if we're in inspect mode
        if ((resultScreen != null))
        {
            resultScreen.SetActive(true);
        }
    }
}
