using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        if (normalImage != null)
            imageComponent.sprite = normalImage;
    }
}
