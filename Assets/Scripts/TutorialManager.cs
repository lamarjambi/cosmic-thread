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
    
    private bool cardClicked = false; 

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
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
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
            
            case 3: // instruction 3: timer!!!
                break;

            default:
                break;
        }
    }
    
    public void OnCardClicked()
    {
        cardClicked = true;
    }
}