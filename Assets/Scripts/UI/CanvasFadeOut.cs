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
            return;
        }
        
        CreateFadePanel();
    }
    
    void CreateFadePanel()
    {
        // for fade panel
        fadePanel = new GameObject("FadePanel");
        fadePanel.transform.SetParent(parentCanvas.transform, false);
        
        fadePanelImage = fadePanel.AddComponent<Image>();
        fadePanelImage.color = fadeColor;
        
        RectTransform rectTransform = fadePanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        fadePanel.transform.SetAsLastSibling();
        
        SetFadeAlpha(0f);
        fadePanel.SetActive(false);
    }
    
    void OnEnable()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut());
    }
    
    void OnDisable()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        
        if (fadePanel != null)
        {
            fadePanel.SetActive(false);
        }
    }
    
    private IEnumerator FadeOut()
    {
        if (fadePanel == null) yield break;
        
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
        
        SetFadeAlpha(1f);
        
        SceneManager.LoadScene("CasesScene", LoadSceneMode.Single);
        
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
    
    public void StartFadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut());
    }
    
    public void ShowBlackScreen()
    {
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
            SetFadeAlpha(1f);
        }
    }
    
    public void HideFadePanel()
    {
        if (fadePanel != null)
        {
            fadePanel.SetActive(false);
        }
    }
}