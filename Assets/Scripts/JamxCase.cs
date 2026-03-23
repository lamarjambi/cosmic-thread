using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JamxCase : HoverableCase, IPointerClickHandler
{
    [Header("Case Settings")]
    public string targetScene = "JamxScene";
    public string description = "Case 02: Something something";
    public AudioClip buttonSound;
    
    [Header("UI Elements")]
    public GameObject descriptionPanel;
    public TMPro.TextMeshProUGUI descriptionText;
    private Image caseImage;
    
    [Header("Description Positioning")]
    public float descriptionWidth = 400f;
    public Vector2 descriptionPosition = new Vector2(-50, -310);
    
    private AudioSource audioSource;
    
    protected override void Start()
    {
        base.Start();

        isLocked = PlayerPrefs.GetInt("ZiggyCaseCompleted", 0) == 0;
        Debug.Log($"[JamxCase] ZiggyCaseCompleted={PlayerPrefs.GetInt("ZiggyCaseCompleted", 0)}, isLocked={isLocked}");

        caseImage = GetComponent<Image>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (descriptionText != null)
            descriptionText.text = description;
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        if (caseImage != null)
            caseImage.color = isLocked ? new Color(0.4f, 0.4f, 0.4f, 1f) : Color.white;
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return;
        base.OnPointerEnter(eventData);
        
        if (descriptionPanel != null)
        {
            RectTransform rectTransform = descriptionPanel.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, descriptionWidth);
            rectTransform.anchoredPosition = descriptionPosition;
            descriptionPanel.SetActive(true);
        }
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return;
        base.OnPointerExit(eventData);
        
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked) return;
        if (hovered && eventData.button == PointerEventData.InputButton.Left)
        {
            if (buttonSound != null && audioSource != null)
                audioSource.PlayOneShot(buttonSound);
            SceneManager.LoadScene(targetScene);
        }
    }
}