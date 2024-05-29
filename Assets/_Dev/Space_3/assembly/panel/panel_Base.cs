using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_Base : MonoBehaviour, IPanel
{
    public ePanelAnimation ePanelAnimation = ePanelAnimation.None;

    private GameObject go_Root;
    private RectTransform rect;

    private Dictionary<ePanelAnimation, Vector2> animationPivot_Appear = new Dictionary<ePanelAnimation, Vector2>();
    private Dictionary<ePanelAnimation, Vector2> animationPivot_DisAppear = new Dictionary<ePanelAnimation, Vector2>();

    protected virtual void Awake()
    {
        go_Root = gameObject.Search(nameof(go_Root)).gameObject;
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

    public virtual void Open()
    {
        gameObject.SetActive(true);
        StartCoroutine(Co_OpenAnimation());
    }
    public virtual void Close()
    {
        StartCoroutine(Co_CloseAnimation());
    }

    IEnumerator Co_OpenAnimation()
    {
        if (ePanelAnimation != ePanelAnimation.None)
        {
            float curTime = 0f;
            float durTime = 0.5f;
            while (curTime < 1f)
            {
                curTime += Time.deltaTime / durTime;
                rect.pivot = Vector3.Lerp(animationPivot_DisAppear[ePanelAnimation], animationPivot_Appear[ePanelAnimation], EasingFunction.EaseInExpo(0f, 1f, curTime));
                yield return null;
            }
        }
    }
    IEnumerator Co_CloseAnimation()
    {
        if (ePanelAnimation != ePanelAnimation.None)
        {
            float curTime = 0f;
            float durTime = 0.5f;
            while (curTime < 1f)
            {
                curTime += Time.deltaTime / durTime;
                rect.pivot = Vector3.Lerp(animationPivot_Appear[ePanelAnimation], animationPivot_DisAppear[ePanelAnimation], EasingFunction.EaseInExpo(0f, 1f, curTime));
                yield return null;
            }
        }
        gameObject.SetActive(false);
    }
}
