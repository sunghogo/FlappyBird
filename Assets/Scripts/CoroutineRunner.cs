using UnityEngine;
using System.Collections;
using TMPro;

public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        transform.SetParent(null);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    static CoroutineRunner EnsureInstance()
    {
        if (Instance) return Instance;
        var go = new GameObject("[CoroutineRunner]");
        go.transform.SetParent(null);
        Instance = go.AddComponent<CoroutineRunner>();
        DontDestroyOnLoad(go);
        return Instance;
    }

    // ---------- PUBLIC START METHODS ----------

    public static Coroutine ChangeStateAfterDelay(float delay, GameState gameState)
    {
      return EnsureInstance().StartCoroutine(ChangeStateAfterDelayRoutine(delay, gameState));
    }


    public static Coroutine StartFadeOutThenDisable(GameObject target, float dur = 1f, float delay = 0f)
    {
        if (!target) return null;
        var srs = target.GetComponentsInChildren<SpriteRenderer>(true);
        if (srs.Length == 0)
        {
            if (delay > 0f) EnsureInstance().StartCoroutine(DisableAfterDelay(target, delay));
            else target.SetActive(false);
            return null;
        }
        return EnsureInstance().StartCoroutine(FadeOutAllThenDisable(target, srs, dur, delay));
    }

    public static Coroutine StartEnableThenFadeIn(GameObject target, float dur = 1f, float delay = 0f)
    {
        if (!target) return null;

        var tmp = target.GetComponent<TMP_Text>() ?? target.GetComponentInChildren<TMP_Text>(true);
        if (tmp) return EnsureInstance().StartCoroutine(EnableThenFadeInTMP(tmp, dur, delay));

        var srs = target.GetComponentsInChildren<SpriteRenderer>(true);
        if (srs.Length > 0) return EnsureInstance().StartCoroutine(EnableThenFadeInAll(target, srs, dur, delay));

        var cg = target.GetComponent<CanvasGroup>() ?? target.AddComponent<CanvasGroup>();
        return EnsureInstance().StartCoroutine(EnableThenFadeInCanvasGroup(cg, dur, delay));
    }

    public static Coroutine StartFadeOutThenDisable(TMP_Text tmp, float dur = 1f, float delay = 0f)
    {
        if (!tmp) return null;
        return EnsureInstance().StartCoroutine(FadeOutTMPThenDisable(tmp, dur, delay));
    }

    public static Coroutine StartEnableThenFadeIn(TMP_Text tmp, float dur = 1f, float delay = 0f)
    {
        if (!tmp) return null;
        return EnsureInstance().StartCoroutine(EnableThenFadeInTMP(tmp, dur, delay));
    }

    // Helper
    static IEnumerator ChangeStateAfterDelayRoutine(float delay, GameState gameState)
    {
        yield return new WaitForSecondsRealtime(delay);
        switch (gameState)
        {
            case (GameState.StartingScreen):
                GameManager.Instance.StartScreen();
                break;
            case (GameState.GameStart):
                GameManager.Instance.StartGame();
                break;
            case (GameState.GameOver):
                GameManager.Instance.EndGame();
                break;
        }
    }

    // ---------- COROUTINES (TMP/UI) ----------
    static IEnumerator FadeOutTMPThenDisable(TMP_Text tmp, float dur, float delay)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);
        dur = Mathf.Max(0.0001f, dur);
        var go = tmp.gameObject;
        Color c = tmp.color; float a0 = c.a; float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            tmp.color = new Color(c.r, c.g, c.b, Mathf.Lerp(a0, 0f, t / dur));
            yield return null;
        }
        tmp.color = new Color(c.r, c.g, c.b, 0f);
        go.SetActive(false);
    }

    static IEnumerator EnableThenFadeInTMP(TMP_Text tmp, float dur, float delay)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);
        dur = Mathf.Max(0.0001f, dur);
        var go = tmp.gameObject; go.SetActive(true);
        Color c = tmp.color; float t = 0f;
        tmp.color = new Color(c.r, c.g, c.b, 0f);
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            tmp.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0f, 1f, t / dur));
            yield return null;
        }
        tmp.color = new Color(c.r, c.g, c.b, 1f);
    }

    static IEnumerator EnableThenFadeInCanvasGroup(CanvasGroup cg, float dur, float delay)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);
        dur = Mathf.Max(0.0001f, dur);
        var go = cg.gameObject; go.SetActive(true);
        cg.alpha = 0f; float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / dur);
            yield return null;
        }
        cg.alpha = 1f;
    }

    // ---------- COROUTINES (SPRITERENDERERS) ----------
    static IEnumerator EnableThenFadeInAll(GameObject root, SpriteRenderer[] srs, float dur, float delay = 0f)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);
        dur = Mathf.Max(0.0001f, dur);

        root.SetActive(true);

        // Set all to 0 alpha to start
        for (int i = 0; i < srs.Length; i++)
        {
            var c = srs[i].color;
            srs[i].color = new Color(c.r, c.g, c.b, 0f);
        }

        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / dur);
            for (int i = 0; i < srs.Length; i++)
            {
                var c = srs[i].color;
                srs[i].color = new Color(c.r, c.g, c.b, Mathf.Lerp(0f, 1f, k)); // <- target 1
            }
            yield return null;
        }

        // Snap to fully opaque
        for (int i = 0; i < srs.Length; i++)
        {
            var c = srs[i].color;
            srs[i].color = new Color(c.r, c.g, c.b, 1f);
        }
    }

    static IEnumerator FadeOutAllThenDisable(GameObject root, SpriteRenderer[] srs, float dur, float delay)
    {
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);
        dur = Mathf.Max(0.0001f, dur);

        var start = new Color[srs.Length];
        for (int i = 0; i < srs.Length; i++) start[i] = srs[i].color;

        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / dur);
            for (int i = 0; i < srs.Length; i++)
            {
                var c = start[i];
                srs[i].color = new Color(c.r, c.g, c.b, Mathf.Lerp(c.a, 0f, k));
            }
            yield return null;
        }
        for (int i = 0; i < srs.Length; i++)
        {
            var c = srs[i].color;
            srs[i].color = new Color(c.r, c.g, c.b, 0f);
        }
        root.SetActive(false);
    }

    // Helper
    static IEnumerator DisableAfterDelay(GameObject go, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        go.SetActive(false);
    }
}
