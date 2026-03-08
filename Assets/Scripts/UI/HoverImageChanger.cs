using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Settings")]
    [SerializeField] private Sprite hoverImage;
    private Image imageComponent;
    private Sprite normalImage;
    
    void Start()
    {
        imageComponent = GetComponent<Image>();
        normalImage = imageComponent.sprite; 
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage != null)
            imageComponent.sprite = hoverImage;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // only be able to hover/click when it's investigate mode
        if (normalImage != null)
            imageComponent.sprite = normalImage;
    }
}