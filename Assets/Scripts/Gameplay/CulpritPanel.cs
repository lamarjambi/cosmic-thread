using UnityEngine;

public class CulpritPanel : MonoBehaviour
{
    [SerializeField] GameObject modeIndicator;
    [SerializeField] GameObject countdownTimer;
    [SerializeField] GameObject gavel;

    [Header("Result")]
    [SerializeField] GameObject resultPanel;

    [SerializeField] GameObject correctCulprit;
    
    private CountdownTimer timer;

    void Start()
    {
        gavel.SetActive(false);
        modeIndicator.SetActive(false);
        countdownTimer.SetActive(false);
        timer = countdownTimer.GetComponent<CountdownTimer>();

        resultPanel.SetActive(false);
    }

    void OnEnable()
    {
        if (timer != null)
            timer.timerIsRunning = false;
    }

    public void OnCulpritSelected(GameObject selected)
    {
        bool correct = selected == correctCulprit;

        if (correct)
        {
            PlayerPrefs.SetInt("ZiggyCaseCompleted", 1);
            PlayerPrefs.Save();
            Debug.Log("ZiggyCaseCompleted set to: " + PlayerPrefs.GetInt("ZiggyCaseCompleted"));
        }

        resultPanel.GetComponent<ResultPanel>().Show(correct);
    }
}