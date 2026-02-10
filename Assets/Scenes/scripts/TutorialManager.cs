using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // tutorial video: https://youtu.be/a1RFxtuTVsk?si=TS1GzoJ3xd0j5-C5
    public GameObject[] popUps; // instructions
    private int popUpIndex;

    void Update()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[i].SetActive(true);
            } else
            {
                popUps[i].SetActive(false);
            }
        }

        // instruction 1: click any of the cards to investigate
        if (popUpIndex == 0) 
        {
            
        }
    }
}
