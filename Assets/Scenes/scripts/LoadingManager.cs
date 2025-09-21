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
    public float minimumLoadTime = 2f; // Minimum time to show loading screen
    
    private string targetScene;
    
    void Start()
    {
        // Get the target scene from PlayerPrefs (set by the case button)
        targetScene = PlayerPrefs.GetString("TargetScene", "BackstoryScene");
        
        StartCoroutine(LoadSceneAsync());
    }
    
    IEnumerator LoadSceneAsync()
    {
        float startTime = Time.time;
        float fakeProgress = 0f;
        
        // Start loading the target scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false; // Don't activate immediately
        
        // Update progress bar while loading
        while (!operation.isDone)
        {
            float elapsedTime = Time.time - startTime;
            
            // Calculate real progress from Unity's async operation (0 to 0.9 during loading)
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // Calculate fake progress based on elapsed time (for smooth visual feedback)
            float timeBasedProgress = Mathf.Clamp01(elapsedTime / minimumLoadTime);
            
            // Use the minimum of real progress and time-based progress to ensure smooth loading
            float displayProgress = Mathf.Min(realProgress, timeBasedProgress);
            
            // If we're past the minimum time and real loading is complete, use real progress
            if (elapsedTime >= minimumLoadTime && operation.progress >= 0.9f)
            {
                displayProgress = 1f;
            }
            
            // Update UI
            if (progressBar != null)
                progressBar.value = displayProgress;
                
            if (loadingText != null)
                loadingText.text = $"Loading... {Mathf.RoundToInt(displayProgress * 100)}%";
            
            // Check if we're ready to activate the scene
            if (operation.progress >= 0.9f && elapsedTime >= minimumLoadTime)
            {
                // Small delay then activate the scene
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}