using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ZiggyCase : HoverableCase, IPointerClickHandler
{
    [Header("Case Settings")]
    public string targetScene = "LoadingScene";
    public string description = "Case 01: Something something";
    public AudioClip buttonSound;
    
    [Header("UI Elements")]
    public GameObject descriptionPanel;
    public TMPro.TextMeshProUGUI descriptionText;
    
    [Header("Description Positioning")]
    public float descriptionWidth = 400f;
    public Vector2 descriptionPosition = new Vector2(-600, -310);
    
    private AudioSource audioSource;
    
    protected override void Start()
    {
        base.Start();
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
            
        // Setup description UI
        if (descriptionText != null)
            descriptionText.text = description;
            
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        
        if (descriptionPanel != null)
        {
            RectTransform rectTransform = descriptionPanel.GetComponent<RectTransform>();
            
            // Set fixed width
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, descriptionWidth);
            
            // Set position
            rectTransform.anchoredPosition = descriptionPosition;
            
            descriptionPanel.SetActive(true);
        }
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (buttonSound != null && audioSource != null)
                audioSource.PlayOneShot(buttonSound);
            
            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(targetScene);
        }
    }
}