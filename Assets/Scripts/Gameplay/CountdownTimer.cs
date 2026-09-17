using UnityEngine;
using TMPro; 
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timeRemaining = 120f;
    public bool timerIsRunning = false;

    [Header("Time Up")]
    [SerializeField] ResultPanel resultPanel;

    private void Start()
    {
        timerText = GetComponent<TMP_Text>(); 
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; 
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("no more TIME!");
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(0f);

                // ran out of time = loss, show the blood screen
                if (resultPanel != null)
                    resultPanel.Show(false);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
