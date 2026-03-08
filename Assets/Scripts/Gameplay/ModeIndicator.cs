using UnityEngine;
using TMPro;

public class ModeIndicator : MonoBehaviour
{
    public bool isThreadMode = false; // false = inspect mode, true = thread mode
    public TMP_Text modeText;
    public float timer = 2f;

    private void Start()
    {
        UpdateModeText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isThreadMode = !isThreadMode;
            UpdateModeText();
        }
    }

    private void UpdateModeText()
    {
        modeText.text = (isThreadMode ? "Thread Mode" : "Inspect Mode");
    }
}
