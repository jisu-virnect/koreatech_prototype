using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class toast_Base : MonoBehaviour, IToast
{
    private TMP_Text tmp_Toast;
    private GameObject go_Root;
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Coroutine coroutine = null;
    public bool isOpen;
    private Action act = null;

    protected virtual void Awake()
    {
        tmp_Toast = gameObject.Search<TMP_Text>(nameof(tmp_Toast));
        go_Root = gameObject.SearchGameObject(nameof(go_Root));
        
        canvasGroup = go_Root.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        
        rect = go_Root.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector3.zero;
    }

    public virtual void Show(string message, Action act = null)
    {
        gameObject.SetActive(true);
        isOpen = true;
        if (act != null)
        {
            this.act = act;
        }

        tmp_Toast.text = message;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(Co_Show());
    }

    public virtual void ShowHide(string message, float duration = 0f, Action act = null)
    {
        gameObject.SetActive(true);
        isOpen = true;
        if (act != null)
        {
            this.act = act;
        }

        tmp_Toast.text = message;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(Co_Animation(duration, act));
    }

    public virtual void Hide(Action act = null)
    {
        if (act!=null)
        {
            this.act = act;
        }
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(Co_Hide());
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

    IEnumerator Co_Animation(float duration = 0f, Action act = null)
    {
        this.act = act;

        yield return Co_Show();
        yield return new WaitForSeconds(duration);
        yield return Co_Hide();
    }
    IEnumerator Co_Show()
    {
        StartCoroutine(Co_Transform(0f, -20f));
        yield return Co_CanvasAlpha(0f, 1f);
    }

    IEnumerator Co_Hide()
    {
        if (!isOpen)
        {
            yield break;
        }
        isOpen = false;
        StartCoroutine(Co_Transform(-20f, 0f));
        yield return Co_CanvasAlpha(1f, 0f);

        if (act != null)
        {
            int hashCode = act.GetHashCode();
            act?.Invoke();
            if(hashCode == act.GetHashCode())
            {
                act = null;
            }
        }
    }

    public virtual void SetData<T>(T t) where T : class
    {

    }
}
