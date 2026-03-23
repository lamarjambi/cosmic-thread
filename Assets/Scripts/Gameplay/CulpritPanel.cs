using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CulpritPanel : MonoBehaviour
{
    [SerializeField] GameObject modeIndicator;
    [SerializeField] GameObject countdownTimer;
    [SerializeField] GameObject gavel;

    [Header("Result")]
    [SerializeField] GameObject resultPanel;
    [SerializeField] GameObject jailImage;
    [SerializeField] GameObject bloodImage;

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
        StartCoroutine(ShowResultAndTransition(correct));
    }

    private IEnumerator ShowResultAndTransition(bool correct)
    {
        resultPanel.SetActive(true);
        jailImage.SetActive(correct);
        bloodImage.SetActive(!correct);

        if (correct)
        {
            PlayerPrefs.SetInt("ZiggyCaseCompleted", 1);
            PlayerPrefs.Save();
            Debug.Log("ZiggyCaseCompleted set to: " + PlayerPrefs.GetInt("ZiggyCaseCompleted"));
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("CasesScene");
    }
}