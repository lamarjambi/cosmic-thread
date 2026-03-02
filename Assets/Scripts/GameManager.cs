using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IPointerClickHandler
{
    private bool isItemClicked = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isItemClicked)
        {
            // draw red line between the items 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isItemClicked = true;
    }
}
