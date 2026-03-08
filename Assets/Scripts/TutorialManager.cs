using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    // tutorial video: https://youtu.be/a1RFxtuTVsk?si=TS1GzoJ3xd0j5-C5
    public static TutorialManager Instance; // week 5 - singleton 
    public GameObject[] popUps; // instructions
    [SerializeField] GameObject textBubble;
    [SerializeField] GameObject resultTextBubble;
    public int popUpIndex;

    [Header("Highlights")]
    [SerializeField] GameObject highlight1;
    [SerializeField] GameObject highlight2;
    [SerializeField] GameObject highlight3;
    [SerializeField] GameObject highlight4;
    [SerializeField] GameObject highlight5;

    [SerializeField] GameObject modeIndicator;
    [SerializeField] ModeIndicator modeInd;
    [SerializeField] GameObject newTimer;
    [SerializeField] GameObject gavel;

    [Header("Triggers")]
    private bool cardClicked = false; 
    private bool gavelClicked = false;
    private bool resultClicked = false;


    void Awake()
    {
        Instance = this; // singleton initiation
    }

    void Start()
    {
        TriggerTypewriter();
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
            case 0: // instruction 0: intro
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 0) 
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    AdvancePopUp();
                }
                break;

            case 1: // instruction 1: intro
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 1)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    highlight1.SetActive(false);
                    highlight2.SetActive(true);
                    AdvancePopUp();
                }
                break;

            case 2: // instruction 2: click any of the cards to investigate
                if (cardClicked)
                {
                    cardClicked = false; // to reset
                    AdvancePopUp();
                }
                break;

            case 3: // instruction 3: click on bubble to proceed
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 3)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false;
                    AdvancePopUp();
                }
                break;

            case 4: // instruction 4: tap for different modes    
                modeIndicator.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    AdvancePopUp();
                }
                break;

            case 5: // instruction 5: shift + click!!!
                highlight2.SetActive(false);
                if (modeInd.isThreadMode && GameManager.Instance.connectionMade)
                {
                    GameManager.Instance.connectionMade = false; // reset
                    AdvancePopUp();
                }
                break;

            case 6: // instruction 6: escape 
                if ((modeInd.isThreadMode) && (Input.GetKeyDown(KeyCode.Escape)))
                {
                    highlight3.SetActive(true);
                    AdvancePopUp();
                }
                break;

            case 7: // instruction 7: timer!! 
                newTimer.SetActive(true);
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 7)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    AdvancePopUp();
                }
                break;

            case 8: // instruction 8: connect correct one!! 
                highlight3.SetActive(false);
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 8)
                {
                    highlight4.SetActive(true);
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    AdvancePopUp();
                }
                break;

            case 9: // instruction 9: you get gavel
                gavel.SetActive(true);
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 9)
                {
                    highlight4.SetActive(false);
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    AdvancePopUp();
                }
                break;

            case 10: // instruction 10: use gavel when you know who the culprit is
                if (gavelClicked)
                {
                    gavelClicked = false; // reset
                    AdvancePopUp();
                }
                break;  

            case 11: // instruction 11: WHO DONE IT?
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 11)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    AdvancePopUp();
                }
                break; 

            case 12: // instruction 12: WHO DONE IT?
                if (resultClicked)
                {
                    resultClicked = false; // to reset
                    AdvancePopUp();
                }
                break;

            case 13: // instruction 13: OUTRO
                textBubble = resultTextBubble;
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 13)
                {
                    textBubble.GetComponent<TextBubbleClick>().wasClicked = false; // reset
                    AdvancePopUp();
                }
                break;

            case 14: // instruction 14: OUTRO + loading to ziggycase
                if (textBubble.GetComponent<TextBubbleClick>().wasClicked && Instance.popUpIndex == 14)
                {
                    PlayerPrefs.SetString("PreviousScene", "TutorialScene");
                    SceneManager.LoadScene("LoadingScene");
                }
                break;                        

            default:
                break;
        }
    }

    private void AdvancePopUp()
    {
        TextAnim anim = popUps[popUpIndex].GetComponent<TextAnim>();
        if (anim != null && anim.IsTyping)
        {
            anim.SkipTypewriter(); // first click skips
            return;
        }

        popUpIndex++;
        TriggerTypewriter();
    }

    private void TriggerTypewriter()
    {
        if (popUpIndex >= popUps.Length) return;

        TextAnim anim = popUps[popUpIndex].GetComponent<TextAnim>();
        if (anim != null)
        {
            anim.StartTypewriter();
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

    public void OnResultClicked()
    {
        resultClicked = true;
    }
}