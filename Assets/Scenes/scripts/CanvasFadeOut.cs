using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasFadeOut : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private bool deactivateAfterFade = false;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    private GameObject fadePanel;
    private Image fadePanelImage;
    private Canvas parentCanvas;
    private Coroutine fadeCoroutine;
    
    void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogError("CanvasFadeOut: No Canvas found in parent hierarchy!");
            return;
        }
        
        CreateFadePanel();
    }
    
    void CreateFadePanel()
    {
        // Create a new GameObject for the fade panel
        fadePanel = new GameObject("FadePanel");
        fadePanel.transform.SetParent(parentCanvas.transform, false);
        
        // Add Image component for the black overlay
        fadePanelImage = fadePanel.AddComponent<Image>();
        fadePanelImage.color = fadeColor;
        
        // Make it cover the entire screen
        RectTransform rectTransform = fadePanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        // Set it as the top-most UI element
        fadePanel.transform.SetAsLastSibling();
        
        // Start invisible
        SetFadeAlpha(0f);
        fadePanel.SetActive(false);
    }
    
    void OnEnable()
    {
        // Start fade out when activated
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut());
    }
    
    void OnDisable()
    {
        // Stop any running fade coroutine
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        
        // Hide fade panel
        if (fadePanel != null)
        {
            fadePanel.SetActive(false);
        }
    }
    
    private IEnumerator FadeOut()
    {
        if (fadePanel == null) yield break;
        
        // Activate the fade panel
        fadePanel.SetActive(true);
        
        float currentTime = 0f;
        
        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / fadeOutDuration;
            float alpha = fadeCurve.Evaluate(normalizedTime);
            
            SetFadeAlpha(alpha);
            
            yield return null;
        }
        
        // Ensure final alpha is 1 (fully black)
        SetFadeAlpha(1f);
        
        // Load the next scene after fade completes
        SceneManager.LoadScene("CasesScene", LoadSceneMode.Single);
        
        // Optionally deactivate this GameObject after fade
        if (deactivateAfterFade)
        {
            gameObject.SetActive(false);
        }
        
        fadeCoroutine = null;
    }
    
    private void SetFadeAlpha(float alpha)
    {
        if (fadePanelImage != null)
        {
            Color color = fadePanelImage.color;
            color.a = alpha;
            fadePanelImage.color = color;
        }
    }
    
    // Public method to manually start fade (if needed)
    public void StartFadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut());
    }
    
    // Public method to instantly show black screen
    public void ShowBlackScreen()
    {
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
            SetFadeAlpha(1f);
        }
    }
    
    // Public method to hide fade panel
    public void HideFadePanel()
    {
        if (fadePanel != null)
        {
            fadePanel.SetActive(false);
        }
    }
}