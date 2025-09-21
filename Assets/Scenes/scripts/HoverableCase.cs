using UnityEngine;
using UnityEngine.EventSystems;

public class HoverableCase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animation Settings")]
    public float hoverScale = 1.08f;
    public float hoverAngle = 7f;
    public float animationSpeed = 0.1f;
    
    protected bool hovered = false;
    protected Vector3 baseScale;
    protected float baseAngle = 0f;
    
    // Target values for animation
    protected Vector3 targetScale;
    protected float targetAngle;
    
    protected virtual void Start()
    {
        baseScale = transform.localScale;
        targetScale = baseScale;
        targetAngle = baseAngle;        
    }
    
    protected virtual void Update()
    {
        // Smooth animations using Lerp
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, animationSpeed);
        
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = Mathf.LerpAngle(currentRotation.z, targetAngle, animationSpeed);
        transform.eulerAngles = currentRotation;
    }
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        targetScale = baseScale * hoverScale;
        targetAngle = hoverAngle;
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        targetScale = baseScale;
        targetAngle = baseAngle;
    }
}