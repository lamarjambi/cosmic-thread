using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider progressBar;
    public TMPro.TextMeshProUGUI loadingText;
    
    [Header("Loading Settings")]
    public float minimumLoadTime = 2f; 
    
    private string targetScene;
    
    void Start()
    {
        string previousScene = PlayerPrefs.GetString("PreviousScene", "");
        
        if (previousScene == "MainMenu") 
        {
            targetScene = "BackstoryScene";
        }
        else if (previousScene == "CasesScene") 
        {
            targetScene = "TutorialScene";
        } else if (previousScene == "TutorialScene") 
        {
            targetScene = "ZiggyScene";
        }
        else
        {
            targetScene = "BackstoryScene";
        }
        
        StartCoroutine(LoadSceneAsync());
    }
    
    IEnumerator LoadSceneAsync()
    {
        float startTime = Time.time;
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false; 
        
        while (!operation.isDone)
        {
            float elapsedTime = Time.time - startTime;
            
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            float timeBasedProgress = Mathf.Clamp01(elapsedTime / minimumLoadTime);
            float displayProgress = Mathf.Min(realProgress, timeBasedProgress);
            
            if (elapsedTime >= minimumLoadTime && operation.progress >= 0.9f)
            {
                displayProgress = 1f;
            }
            
            if (progressBar != null)
                progressBar.value = displayProgress;
                
            if (loadingText != null)
                loadingText.text = $"Loading... {Mathf.RoundToInt(displayProgress * 100)}%";
            
            if (operation.progress >= 0.9f && elapsedTime >= minimumLoadTime)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}