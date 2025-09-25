using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialOverlay : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public GameObject instructor; // the character giving instructions
    public GameObject speechBubble;
    public TMPro.TextMeshProUGUI instructionText;
    public GameObject greyOverlay; // panel that covers the screen
    public GameObject clickToContinueIndicator; // shows "Click to continue" text
    public TMPro.TextMeshProUGUI modeIndicator; // shows current mode (Inspect/Thread)
    
    [Header("Items to Highlight")]
    public ItemTutorial[] boardItems; // dummy, dummy2, dummy3
    public GameObject[] backgroundElements; // elements to grey out
    
    [Header("Layering")]
    public Canvas tutorialCanvas; // the canvas for the tutorial overlay
    public Canvas boardCanvas; // the canvas for the board items
    public Transform tutorialOverlayParent; // the tutorialOverlay parent object
    
    // tutorial state
    private MysteryTutorial gameManager;
    private bool waitingForItemClick = false;
    private bool waitingForExitClick = false;
    private bool speechBubbleClickable = false;
    private bool waitingForTabPress = false;
    private bool waitingForShiftClick = false;
    
    // tutorial step messages
    private readonly string[] tutorialMessages = {
        "Welcome to Cosmic Thread! I'll teach you how to solve mysteries.",
        "Click any of the items to investigate the case. Look closely for clues!",
        "You are currently in Inspect Mode! That's how you were able to click on the items", // player should click on the bubble to proceed in text
        "To exit Inspect Mode, press TAB and enter Thread Mode",
        "Connect the clues that you think are related to each other by holding shift and clicking the items!"
    };
    
    void Start()
    {
        gameManager = FindObjectOfType<MysteryTutorial>();
        
        // initialize ui state
        if (greyOverlay != null) greyOverlay.SetActive(true);
        
        SetAllItemsGreyedOut(true);
    }
    
    void Update()
    {
        // handle TAB key press for mode switching
        if (waitingForTabPress && Input.GetKeyDown(KeyCode.Tab))
        {
            waitingForTabPress = false;
            if (gameManager != null)
            {
                gameManager.OnTabPressed();
            }
        }
    }
    
    public void ShowStep1Introduction()
    {
        SetInstructorVisible(true);
        SetSpeechBubble(tutorialMessages[0], true); // show click to continue
        SetAllItemsGreyedOut(true);
    }
    
    public void ShowStep2InspectionDemo()
    {
        SetSpeechBubble(tutorialMessages[1], false); // hide click to continue, must do task
        HighlightBoardItems(true);
        waitingForItemClick = true;
        
        // move dummy items in front of tutorialOverlay in hierarchy
        MoveDummiesInFrontOfOverlay();
        
        // enable clicking on board items
        foreach (var item in boardItems)
        {
            item.SetInteractable(true);
            item.OnItemClicked += OnDemoItemClicked;
        }
    }
    
    public void ShowStep3ModeIntroduction()
    {
        SetInstructorVisible(true);
        SetSpeechBubble(tutorialMessages[2], true); // show click to continue
        SetModeIndicator("Inspect Mode");
    }
    
    public void ShowStep3TabInstruction()
    {
        SetSpeechBubble(tutorialMessages[3], false); // hide click to continue, must press TAB
        waitingForTabPress = true;
    }
    
    public void ShowStep3ThreadModeInstruction()
    {
        SetSpeechBubble(tutorialMessages[4], false); // hide click to continue, must shift+click
        SetModeIndicator("Thread Mode");
        waitingForShiftClick = true;
        
        // enable clicking on board items for shift+click
        foreach (var item in boardItems)
        {
            item.SetInteractable(true);
            item.OnItemClicked += OnShiftClickDemo;
        }
    }
    
    public void HideInstructor()
    {
        if (greyOverlay != null) greyOverlay.SetActive(false);
        
        // put tutorial overlay behind everything (step 3 - ungrey everything)
        PutOverlayBehindEverything();
    }
    
    void OnDemoItemClicked()
    {
        if (!waitingForItemClick) return;
        
        waitingForItemClick = false;
        waitingForExitClick = true;
        
        // disable clicking on board items during transition
        foreach (var item in boardItems)
        {
            item.SetInteractable(false);
            item.OnItemClicked -= OnDemoItemClicked;
            // subscribe to exit click events
            item.OnExitClicked += OnExitClicked;
        }
    }
    
    void OnExitClicked()
    {
        if (!waitingForExitClick) return;
        
        waitingForExitClick = false;
        
        // unsubscribe from exit click events
        foreach (var item in boardItems)
        {
            item.OnExitClicked -= OnExitClicked;
        }
        
        // move all items on top of tutorial overlay and complete tutorial
        MoveAllItemsOnTopOfOverlay();
        
        // advance to next step after a short delay
        StartCoroutine(AdvanceAfterInspectionDemo());
    }
    
    void OnShiftClickDemo()
    {
        if (!waitingForShiftClick) return;
        
        // check if shift is being held
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) return;
        
        waitingForShiftClick = false;
        
        // disable clicking during transition
        foreach (var item in boardItems)
        {
            item.SetInteractable(false);
            item.OnItemClicked -= OnShiftClickDemo;
        }
        
        // advance to next step after a short delay
        StartCoroutine(AdvanceAfterShiftClickDemo());
    }
    
    IEnumerator AdvanceAfterInspectionDemo()
    {
        yield return new WaitForSeconds(1f);
        
        if (gameManager != null)
        {
            gameManager.OnInspectionDemoComplete();
        }
    }
    
    IEnumerator AdvanceAfterShiftClickDemo()
    {
        yield return new WaitForSeconds(1f);
        
        if (gameManager != null)
        {
            gameManager.OnShiftClickDemoComplete();
        }
    }
    
    
    void SetInstructorVisible(bool visible)
    {
        if (instructor != null) instructor.SetActive(visible);
    }
    
    void SetModeIndicator(string mode)
    {
        if (modeIndicator != null) modeIndicator.text = mode;
    }
    
    void SetSpeechBubble(string message, bool showClickToContinue)
    {
        if (speechBubble != null) speechBubble.SetActive(true);
        if (instructionText != null) instructionText.text = message;
        
        // show "Click to continue" indicator for clickable speech bubbles
        if (showClickToContinue && clickToContinueIndicator != null)
        {
            Text indicatorText = clickToContinueIndicator.GetComponent<Text>();
            if (indicatorText != null)
            {
                indicatorText.text = "Click to continue...";
                indicatorText.color = Color.white;
            }
            clickToContinueIndicator.SetActive(true);
        }
        else if (!showClickToContinue && clickToContinueIndicator != null)
        {
            clickToContinueIndicator.SetActive(false);
        }
        
        // make speech bubble clickable only when showClickToContinue is true
        speechBubbleClickable = showClickToContinue;
    }
    
    void SetAllItemsGreyedOut(bool greyedOut)
    {
        // Grey out background elements
        foreach (var element in backgroundElements)
        {
            if (element != null)
            {
                CanvasGroup cg = element.GetComponent<CanvasGroup>();
                if (cg == null) cg = element.AddComponent<CanvasGroup>();
                cg.alpha = greyedOut ? 0.3f : 1f;
            }
        }
        
        // board items are now handled by hierarchy movement, not individual greying
    }
    
    void HighlightBoardItems(bool highlight)
    {
        // highlighting is now handled by hierarchy movement in MoveDummiesInFrontOfOverlay()
        // no need to call methods on individual items
    }
    
    // Animation methods for smooth transitions
    public void AnimateInstructorEntrance()
    {
        if (instructor != null)
        {
            StartCoroutine(SlideInFromLeft(instructor));
        }
    }
    
    IEnumerator SlideInFromLeft(GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt != null)
        {
            Vector3 startPos = rt.anchoredPosition;
            Vector3 offScreenPos = startPos + Vector3.left * 500f;
            
            rt.anchoredPosition = offScreenPos;
            obj.SetActive(true);
            
            float duration = 0.5f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                rt.anchoredPosition = Vector3.Lerp(offScreenPos, startPos, t);
                yield return null;
            }
            
            rt.anchoredPosition = startPos;
        }
    }
    
    public void AnimateGreyOverlay(bool fadeIn, float duration = 0.3f)
    {
        if (greyOverlay != null)
        {
            StartCoroutine(FadeOverlay(fadeIn, duration));
        }
    }
    
    IEnumerator FadeOverlay(bool fadeIn, float duration)
    {
        CanvasGroup cg = greyOverlay.GetComponent<CanvasGroup>();
        if (cg == null) cg = greyOverlay.AddComponent<CanvasGroup>();
        
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        
        if (fadeIn) greyOverlay.SetActive(true);
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        
        cg.alpha = endAlpha;
        if (!fadeIn) greyOverlay.SetActive(false);
    }
    
    // handle speech bubble clicks
    public void OnPointerClick(PointerEventData eventData)
    {
        if (speechBubbleClickable && speechBubble != null)
        {
            // check if click was on speech bubble
            if (eventData.pointerCurrentRaycast.gameObject == speechBubble || 
                speechBubble.transform.IsChildOf(eventData.pointerCurrentRaycast.gameObject.transform))
            {
                OnSpeechBubbleClicked();
            }
        }
    }
    
    void OnSpeechBubbleClicked()
    {
        if (gameManager != null)
        {
            gameManager.OnSpeechBubbleClicked();
        }
    }
    
    // hierarchy movement methods
    void MoveDummiesInFrontOfOverlay()
    {
        if (tutorialOverlayParent != null)
        {
            // move each dummy item to be siblings of tutorialOverlay, positioned after it
            // this puts them above the tutorialOverlay (including greyTutorial) but below instructions
            foreach (var item in boardItems)
            {
                if (item != null)
                {
                    // move item to be a sibling after tutorialOverlay in Canvas children
                    item.transform.SetParent(tutorialOverlayParent.parent, false);
                    item.transform.SetSiblingIndex(tutorialOverlayParent.GetSiblingIndex());
                }
            }
        }
    }
    
    void MoveAllItemsOnTopOfOverlay()
    {
        if (tutorialOverlayParent != null)
        {
            // move each board item to be siblings of tutorialOverlay, positioned after it
            // this puts them above the grey overlay but below the instructor
            foreach (var item in boardItems)
            {
                if (item != null)
                {
                    // move item to be a sibling after tutorialOverlay in Canvas children
                    item.transform.SetParent(tutorialOverlayParent.parent, false);
                    item.transform.SetSiblingIndex(tutorialOverlayParent.GetSiblingIndex());
                }
            }
            
            // ensure the instructions parent (which contains instructor) stays on top
            if (instructor != null && instructor.transform.parent != null)
            {
                instructor.transform.parent.SetAsLastSibling();
            }
            
            // put tutorial overlay behind everything (but instructor stays on top)
            PutOverlayBehindEverything();
        }
    }
    
    void PutOverlayBehindEverything()
    {
        if (tutorialOverlayParent != null)
        {
            // move tutorialOverlay to be the first child (behind everything else)
            tutorialOverlayParent.SetAsFirstSibling();
        }
        
        if (tutorialCanvas != null)
        {
            // set tutorial canvas to render behind everything
            tutorialCanvas.sortingOrder = -1;
        }
    }
}