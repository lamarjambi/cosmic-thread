using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] GameObject culpritPanel;
    void Start()
    {
        culpritPanel.SetActive(false); 
    }
}
