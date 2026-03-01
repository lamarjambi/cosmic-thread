using UnityEngine;
using TMPro;

public class ModeIndicator : MonoBehaviour
{
    public static bool threadmode = false; // false = inspect mode, true = thread mode
    public TMP_Text modeText;
    public float timer = 2f;

    private void Start()
    {
        UpdateModeText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            threadmode = !threadmode;
            UpdateModeText();
        }
    }

    private void UpdateModeText()
    {
        modeText.text = (threadmode ? "Thread Mode" : "Inspect Mode");
    }
}
