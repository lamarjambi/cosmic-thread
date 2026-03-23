using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialCulpritClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject resultScreen;

    void Start()
    {
        if (resultScreen != null)
            resultScreen.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.popUpIndex != 12)
            return;

        if (resultScreen != null)
            resultScreen.SetActive(true);

        TutorialManager.Instance.OnResultClicked();
    }
}