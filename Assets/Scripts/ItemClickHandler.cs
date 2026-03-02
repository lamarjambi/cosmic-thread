using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemClickHandler : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    [SerializeField] ModeIndicator modeIndicator;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (modeIndicator.isThreadMode)
        {
            GameManager.Instance.OnItemClicked(this.gameObject);
        }
    }
}