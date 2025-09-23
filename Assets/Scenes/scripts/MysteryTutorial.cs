using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MysteryTutorial : MonoBehaviour
{
    [Header("Tutorial References")]
    public TutorialOverlay tutorialOverlay;
    public ItemTutorial[] items; // dummy, dummy2, dummy3
    
    // tutorial state
    public enum TutorialStep { 
        Introduction, 
        ShowInspection, 
        Completed 
    }
    
    [Header("Current State")]
    public TutorialStep currentStep = TutorialStep.Introduction;
    
    void Start()
    {
        // start tutorial
        StartTutorial();
    }
    
    public void StartTutorial()
    {
        currentStep = TutorialStep.Introduction;
        tutorialOverlay.ShowStep1Introduction();
    }
    
    public void AdvanceToInspectionDemo()
    {
        currentStep = TutorialStep.ShowInspection;
        tutorialOverlay.ShowStep2InspectionDemo();
    }
    
    public void CompleteTutorial()
    {
        currentStep = TutorialStep.Completed;
        tutorialOverlay.HideInstructor();
    }
    
    // public method for tutorial overlay to call when inspection demo is complete
    public void OnInspectionDemoComplete()
    {
        CompleteTutorial();
    }
    
    // method called when speech bubble is clicked
    public void OnSpeechBubbleClicked()
    {
        switch (currentStep)
        {
            case TutorialStep.Introduction:
                // advance to inspection demo when speech bubble is clicked
                AdvanceToInspectionDemo();
                break;
                
            case TutorialStep.ShowInspection:
                // cannot advance by speech bubble - must click items first
                break;
                
            default:
                break;
        }
    }
}