using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class toast_Base : MonoBehaviour, IToast
{
    private TMP_Text tmp_Toast;
    private GameObject go_Root;
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Coroutine coroutine = null;

    private void Awake()
    {
        tmp_Toast = gameObject.Search<TMP_Text>(nameof(tmp_Toast));
        go_Root = gameObject.Search(nameof(go_Root)).gameObject;
        
        canvasGroup = go_Root.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        
        rect = go_Root.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector3.zero;
    }

    public virtual void Show(string message, float duration = 0f, Action act = null)
    {
        tmp_Toast.text = message;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(Co_Show(duration, act));
    }

    IEnumerator Co_CanvasAlpha(float start, float end)
    {
        float curTime;
        float durTime = 0.2f;

        curTime = 0f;
        while (curTime < 1f)
        {
            curTime += Time.deltaTime / durTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, curTime);
            yield return null;
        }
        canvasGroup.alpha = end;
    }


    IEnumerator Co_Transform(float start, float end)
    {
        float curTime;
        float durTime = 0.2f;

        curTime = 0f;
        while (curTime < 1f)
        {
            curTime += Time.deltaTime / durTime;
            rect.anchoredPosition = Vector3.up * Mathf.Lerp(start, end, curTime);
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    IEnumerator Co_Show(float duration = 0f, Action act = null)
    {
        StartCoroutine(Co_Transform(0f, -20f));
        yield return Co_CanvasAlpha(0f, 1f);
        if (duration == 0f)
        {
            yield break;
        }
        yield return new WaitForSeconds(duration);
        StartCoroutine(Co_Transform(-20f, 0f));
        yield return Co_CanvasAlpha(1f, 0f);
        act?.Invoke();
    }
}
