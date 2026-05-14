using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.25f;

    [Header("Scale Pop")]
    [SerializeField] private bool useScalePop = true;
    [SerializeField] private float hiddenScale = 0.95f;
    [SerializeField] private float visibleScale = 1f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    public float FadeDuration => fadeDuration;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (useScalePop)
        {
            transform.localScale = Vector3.one * hiddenScale;
        }
    }

    public void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError($"{gameObject.name} is inactive in hierarchy. Check if Canvas or parent is inactive.");
            return;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeRoutine(1f, true));
    }

    public void Hide()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeRoutine(0f, false));
    }

    private IEnumerator FadeRoutine(float targetAlpha, bool enableInteraction)
    {
        float startAlpha = canvasGroup.alpha;
        float timer = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one * (targetAlpha > 0f ? visibleScale : hiddenScale);

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            if (useScalePop)
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            }

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (useScalePop)
        {
            transform.localScale = targetScale;
        }

        canvasGroup.interactable = enableInteraction;
        canvasGroup.blocksRaycasts = enableInteraction;

        if (targetAlpha <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}