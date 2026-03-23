using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class TutorialCulpritPanel : MonoBehaviour
{
    [SerializeField] GameObject modeIndicator;
    [SerializeField] GameObject countdownTimer;
    [SerializeField] GameObject gavel;
    private CountdownTimer timer;
    void Start()
    {
        gavel.SetActive(false); 
        modeIndicator.SetActive(false);
        countdownTimer.SetActive(false);
        timer = countdownTimer.GetComponent<CountdownTimer>();
    }
    void OnEnable()
    {
        if (timer != null)
            timer.timerIsRunning = false;
    }
}