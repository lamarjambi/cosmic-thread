using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleClick : MonoBehaviour, IPointerClickHandler
{
    private MysteryTutorial tutorialManager;
    
    void Start()
    {
        tutorialManager = FindObjectOfType<MysteryTutorial>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (tutorialManager != null)
        {
            tutorialManager.OnSpeechBubbleClicked();
        }
    }
}
