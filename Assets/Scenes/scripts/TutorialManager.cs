using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // tutorial video: https://youtu.be/a1RFxtuTVsk?si=TS1GzoJ3xd0j5-C5
    public static TutorialManager Instance; // week 3 - singleton 
    public GameObject[] popUps; // instructions
    public int popUpIndex;
    
    private bool cardClicked = false; 

    void Awake()
    {
        Instance = this; // singleton initiation
    }

    void Update()
{
    // Show/hide popups
    for (int i = 0; i < popUps.Length; i++)
    {
        popUps[i].SetActive(i == popUpIndex);
    }

    // week 3 - switch system
    switch (popUpIndex)
    {
        case 0: // instruction 1: click any of the cards to investigate
            if (cardClicked)
            {
                popUpIndex++;
                cardClicked = false; // to reset
            }
            break;
            
        case 1: // instruction 2: tap for different modes    
            // code for different modes
            // popUpIndex++;
            break;
            
        case 2: // instruction 3: ..
            // code
            // popUpIndex++;
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