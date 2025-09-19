using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoomCase : HoverableCase, IPointerClickHandler
{
    [Header("Case Settings")]
    public string targetScene = "LoomScene";
    public string description = "Case 03: Something something";
    public AudioClip buttonSound;
    
    [Header("UI Elements")]
    public GameObject descriptionPanel;
    public TMPro.TextMeshProUGUI descriptionText;
    
    private AudioSource audioSource;
    
    protected override void Start()
    {
        base.Start(); // Call parent Start
        
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
        base.OnPointerEnter(eventData); // Call parent hover behavior
        
        // Show description
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData); // Call parent hover exit behavior
        
        // Hide description
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (hovered && eventData.button == PointerEventData.InputButton.Left)
        {
            // Play sound
            if (buttonSound != null && audioSource != null)
                audioSource.PlayOneShot(buttonSound);
                
            // Change scene
            SceneManager.LoadScene(targetScene);
        }
    }
}