using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_Base : MonoBehaviour, IPanel
{
    public Action act_opend;
    protected virtual IEnumerator Action_Opening()
    {
        yield return null;
    }
    protected virtual void Action_Opened()
    {

    }
    public ePanelAnimation ePanelAnimation = ePanelAnimation.None;

    public GameObject go_Root { get; private set; }
    private RectTransform rect;

    private Dictionary<ePanelAnimation, Vector2> animationPivot_Appear = new Dictionary<ePanelAnimation, Vector2>();
    private Dictionary<ePanelAnimation, Vector2> animationPivot_DisAppear = new Dictionary<ePanelAnimation, Vector2>();

    protected virtual void Awake()
    {
        go_Root = gameObject.SearchGameObject(nameof(go_Root)).gameObject;
        if (go_Root != null )
        {
            rect = go_Root.GetComponent<RectTransform>();
        }
        animationPivot_Appear.Add(ePanelAnimation.Left, new Vector2(0f, 0.5f));
        animationPivot_DisAppear.Add(ePanelAnimation.Left, new Vector2(1f, 0.5f));

        animationPivot_Appear.Add(ePanelAnimation.Right, new Vector2(1f, 0.5f));
        animationPivot_DisAppear.Add(ePanelAnimation.Right, new Vector2(0f, 0.5f));

        animationPivot_Appear.Add(ePanelAnimation.Top, new Vector2(0.5f,1f));
        animationPivot_DisAppear.Add(ePanelAnimation.Top, new Vector2(0.5f,0f));

        animationPivot_Appear.Add(ePanelAnimation.Bottom, new Vector2(0.5f,0f));
        animationPivot_DisAppear.Add(ePanelAnimation.Bottom, new Vector2(0.5f,1f));
    }

    public virtual void Open(Action act = null)
    {
        gameObject.SetActive(true);
        StartCoroutine(Co_OpenAnimation(act));
    }
    public virtual void Close(Action act = null)
    {
        StartCoroutine(Co_CloseAnimation(act));
    }

    IEnumerator Co_OpenAnimation(Action act = null)
    {
        if (ePanelAnimation != ePanelAnimation.None)
        {
            StartCoroutine(Action_Opening());
            float curTime = 0f;
            float durTime = 0.3f;
            while (curTime < 1f)
            {
                curTime += Time.deltaTime / durTime;
                rect.pivot = Vector3.Lerp(animationPivot_DisAppear[ePanelAnimation], animationPivot_Appear[ePanelAnimation], EasingFunction.EaseOutExpo(0f, 1f, curTime));
                yield return null;
            }
            act?.Invoke();
            Action_Opened();
        }
    }

    IEnumerator Co_CloseAnimation(Action act = null)
    {
        if (ePanelAnimation != ePanelAnimation.None)
        {
            float curTime = 0f;
            float durTime = 0.2f;
            while (curTime < 1f)
            {
                curTime += Time.deltaTime / durTime;
                rect.pivot = Vector3.Lerp(animationPivot_Appear[ePanelAnimation], animationPivot_DisAppear[ePanelAnimation], EasingFunction.EaseInExpo(0f, 1f, curTime));
                yield return null;
            }
            act?.Invoke();
        }
        gameObject.SetActive(false);
    }

    public virtual void SetData<T>(T t) where T : class
    {
    }
}
