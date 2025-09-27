using System.Collections;
using System.Collections.Generic;
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
        ModeIntroduction,
        TabInstruction,
        ThreadModeInstruction,
        Completed 
    }
    
    [Header("Current State")]
    public TutorialStep currentStep = TutorialStep.Introduction;
    
    [Header("Thread Mode System")]
    public bool threadMode = false; // false = inspect, true = connect
    public List<ConnectionPair> connections = new List<ConnectionPair>();
    public ItemTutorial lastSelected = null;
    public float modeTextTimer = 0f; // timer for mode indicator display
    
    [Header("Visual Feedback")]
    public LineRenderer connectionLinePrefab; // Prefab for drawing connection lines
    public Transform connectionLinesParent; // Parent object for connection lines
    private List<LineRenderer> connectionLines = new List<LineRenderer>();
    
    [System.Serializable]
    public class ConnectionPair
    {
        public ItemTutorial item1;
        public ItemTutorial item2;
        
        public ConnectionPair(ItemTutorial i1, ItemTutorial i2)
        {
            item1 = i1;
            item2 = i2;
        }
        
        public bool IsSamePair(ItemTutorial i1, ItemTutorial i2)
        {
            return (item1 == i1 && item2 == i2) || (item1 == i2 && item2 == i1);
        }
    }
    
    void Start()
    {
        // start tutorial
        StartTutorial();
    }
    
    void Update()
    {
        // Only handle thread mode after tutorial allows it
        if (currentStep == TutorialStep.TabInstruction || 
            currentStep == TutorialStep.ThreadModeInstruction || 
            currentStep == TutorialStep.Completed)
        {
            HandleThreadModeLogic();
        }
    }
    
    void HandleThreadModeLogic()
    {
        // Toggle modes with Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            threadMode = !threadMode;
            modeTextTimer = 1f; // show for 1 second
            lastSelected = null; // reset selection when switching modes
            
            // Update mode indicator
            if (tutorialOverlay != null)
            {
                string modeText = threadMode ? "Thread Mode" : "Inspect Mode";
                tutorialOverlay.SetModeIndicator(modeText);
                tutorialOverlay.ShowModeIndicator(true);
            }
        }
        
        // Decrease mode text timer
        if (modeTextTimer > 0)
        {
            modeTextTimer -= Time.deltaTime;
        }
        
        // Delete key clears all connections
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            connections.Clear();
            UpdateConnectionLines();
        }
        
        // Only allow connections in thread mode while holding shift
        if (threadMode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            // Connection logic will be handled by ItemTutorial when clicked
        }
        else
        {
            lastSelected = null; // reset if shift not held
        }
    }
    
    public void HandleThreadModeClick(ItemTutorial clickedItem)
    {
        // Only allow connections in thread mode and when shift is held
        if (!threadMode || (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)))
        {
            return;
        }
        
        // First click: store the first item
        if (lastSelected == null)
        {
            lastSelected = clickedItem;
            Debug.Log($"Selected first item: {clickedItem.itemName}");
        }
        // Second click: connect with previous
        else if (clickedItem != lastSelected)
        {
            // Check if connection already exists
            bool alreadyConnected = false;
            foreach (var connection in connections)
            {
                if (connection.IsSamePair(lastSelected, clickedItem))
                {
                    alreadyConnected = true;
                    break;
                }
            }
            
            if (!alreadyConnected)
            {
                connections.Add(new ConnectionPair(lastSelected, clickedItem));
                UpdateConnectionLines();
                Debug.Log($"Connected {lastSelected.itemName} with {clickedItem.itemName}");
            }
            
            lastSelected = null; // Reset selection
        }
    }
    
    public bool IsConnected(ItemTutorial item1, ItemTutorial item2)
    {
        foreach (var connection in connections)
        {
            if (connection.IsSamePair(item1, item2))
            {
                return true;
            }
        }
        return false;
    }
    
    void UpdateConnectionLines()
    {
        // Clear existing lines
        foreach (var line in connectionLines)
        {
            if (line != null)
            {
                DestroyImmediate(line.gameObject);
            }
        }
        connectionLines.Clear();
        
        // Create new lines for each connection
        foreach (var connection in connections)
        {
            if (connection.item1 != null && connection.item2 != null)
            {
                CreateConnectionLine(connection.item1, connection.item2);
            }
        }
    }
    
    void CreateConnectionLine(ItemTutorial item1, ItemTutorial item2)
    {
        if (connectionLinePrefab == null) return;
        
        // Create new line renderer
        GameObject lineObj = Instantiate(connectionLinePrefab.gameObject, connectionLinesParent);
        LineRenderer line = lineObj.GetComponent<LineRenderer>();
        
        if (line != null)
        {
            // Set line properties for red connection line
            line.positionCount = 2;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.material.color = Color.red;
            line.sortingOrder = 1; // Make sure it renders on top
            
            // Set line positions (offset like in GameMaker code)
            Vector3 pos1 = item1.transform.position + new Vector3(30, 20, 0);
            Vector3 pos2 = item2.transform.position + new Vector3(30, 20, 0);
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);
            
            connectionLines.Add(line);
        }
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
        AdvanceToModeIntroduction();
    }
    
    public void AdvanceToModeIntroduction()
    {
        currentStep = TutorialStep.ModeIntroduction;
        tutorialOverlay.ShowStep3ModeIntroduction();
    }
    
    public void AdvanceToTabInstruction()
    {
        currentStep = TutorialStep.TabInstruction;
        tutorialOverlay.ShowStep3TabInstruction();
    }
    
    public void AdvanceToThreadModeInstruction()
    {
        currentStep = TutorialStep.ThreadModeInstruction;
        tutorialOverlay.ShowStep3ThreadModeInstruction();
    }
    
    public void OnTabPressed()
    {
        AdvanceToThreadModeInstruction();
    }
    
    public void OnShiftClickDemoComplete()
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
                
            case TutorialStep.ModeIntroduction:
                // advance to TAB instruction when speech bubble is clicked
                AdvanceToTabInstruction();
                break;
                
            case TutorialStep.TabInstruction:
                // cannot advance by speech bubble - must press TAB first
                break;
                
            case TutorialStep.ThreadModeInstruction:
                // cannot advance by speech bubble - must shift+click first
                break;
                
            default:
                break;
        }
    }
}