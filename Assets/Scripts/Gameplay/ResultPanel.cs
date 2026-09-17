using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] GameObject culpritPanel;

    [Header("Result Screens")]
    [SerializeField] GameObject jailImage;
    [SerializeField] GameObject bloodImage;

    [Header("Timing")]
    [SerializeField] float fadeInDuration = 1.5f;
    [SerializeField] float holdDuration = 3f;
    [SerializeField] string nextScene = "CasesScene";

    private CanvasGroup canvasGroup;
    private bool resultShown = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // won = jail screen, lost = blood screen
    public void Show(bool won)
    {
        if (resultShown) return;
        resultShown = true;

        gameObject.SetActive(true);

        if (culpritPanel != null) culpritPanel.SetActive(false);
        if (jailImage != null) jailImage.SetActive(won);
        if (bloodImage != null) bloodImage.SetActive(!won);

        StartCoroutine(FadeInAndTransition());
    }

    private IEnumerator FadeInAndTransition()
    {
        canvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(holdDuration);
        SceneManager.LoadScene(nextScene);
    }
}
