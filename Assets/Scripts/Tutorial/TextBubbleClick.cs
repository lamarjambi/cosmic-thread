using UnityEngine;
using UnityEngine.EventSystems;

public class TextBubbleClick : MonoBehaviour, IPointerClickHandler
{
    public bool wasClicked = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        wasClicked = true;
    }
}