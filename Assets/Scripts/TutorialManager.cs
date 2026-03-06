using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // tutorial video: https://youtu.be/a1RFxtuTVsk?si=TS1GzoJ3xd0j5-C5
    public static TutorialManager Instance; // week 5 - singleton 
    public GameObject[] popUps; // instructions
    [SerializeField] GameObject textBubble;
    public int popUpIndex;
    [SerializeField] GameObject modeIndicator;
    [SerializeField] ModeIndicator modeInd;
    [SerializeField] GameObject newTimer;
    [SerializeField] GameObject gavel;
    
    private bool cardClicked = false; 
    private bool gavelClicked = false;
    

    void Awake()
    {
        Instance = this; // singleton initiation
    }

    void Update()
    {
        
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == popUpIndex);
        }

        // week 3 - switch system
        switch (popUpIndex)
        {
            case 0: // instruction 0: click any of the cards to investigate
                if (cardClicked)
                {
                    popUpIndex++;
                    cardClicked = false; // to reset
                }
                break;
                
            case 1: // instruction 1: click on bubble to proceed
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false;
                    popUpIndex++;
                }
                break;
                
            case 2: // instruction 2: tap for different modes    
                modeIndicator.SetActive(true);
                if (Input.GetKeyDown (KeyCode.Tab)) 
                {
                    popUpIndex++; 
                }
                break;
            
            case 3: // instruction 3: shift + click!!!
                // maybe find a way to make it shift and click
                if (modeInd.isThreadMode && GameManager.Instance.connectionMade)
                {
                    GameManager.Instance.connectionMade = false; // reset
                    popUpIndex++;
                }
                break;

            case 4: // instruction 4: escape 
                if ((modeInd.isThreadMode) && (Input.GetKeyDown(KeyCode.Escape)))
                {
                    popUpIndex++;
                }
                break;

            case 5: // instruction 5: timer!! 
                newTimer.SetActive(true);
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    popUpIndex++;
                }
                break;

            case 6: // instruction 6: connect correct one!! 
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    popUpIndex++;
                }
                break;

            case 7: // instruction 7: you get gavel
                gavel.SetActive(true);
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    popUpIndex++;
                }
                break;

            case 8: // instruction 8: use gavel when you know who the culprit is
                if (gavelClicked)
                {
                    gavelClicked = false; // reset
                    popUpIndex++;
                }
                break;    

            default:
                break;
        }
    }
    
    public void OnCardClicked()
    {
        cardClicked = true;
    }

    public void OnGavelClicked()
    {
        gavelClicked = true;
    }
}