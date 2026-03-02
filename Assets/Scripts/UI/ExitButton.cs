using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour, IPointerClickHandler
{
    // :class: when to show the button 
    [SerializeField] private GameObject panelToClose;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
        }
    }
}