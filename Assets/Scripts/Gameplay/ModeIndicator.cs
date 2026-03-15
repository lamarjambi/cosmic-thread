using UnityEngine;
using TMPro;
using System.Collections;

public class ModeIndicator : MonoBehaviour
{
    public bool isThreadMode = false;
    public TMP_Text modeText;
    public float flashDuration = 0.3f;
    public int flashCount = 2;

    private void Start()
    {
        UpdateModeText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isThreadMode = !isThreadMode;
            UpdateModeText();
            StopCoroutine(nameof(FlashText)); // prevent overlap if spammed
            StartCoroutine(nameof(FlashText));
        }
    }

    private void UpdateModeText()
    {
        modeText.text = isThreadMode ? "Thread Mode" : "Inspect Mode";
    }

    private IEnumerator FlashText()
    {
        Color original = modeText.color;

        for (int i = 0; i < flashCount; i++)
        {
            // Fade out
            yield return LerpAlpha(original.a, 0f, flashDuration / 2);
            // Fade in
            yield return LerpAlpha(0f, original.a, flashDuration / 2);
        }

        modeText.color = original; // ensure we end clean
    }

    private IEnumerator LerpAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = modeText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / duration);
            modeText.color = c;
            yield return null;
        }

        c.a = to;
        modeText.color = c;
    }
}