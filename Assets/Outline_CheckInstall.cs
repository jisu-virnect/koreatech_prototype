using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline_CheckInstall : MonoBehaviour
{
    public float st = 0.5f;
    public float en = 1f;
    private void OnEnable()
    {
        StartCoroutine(Co_Outline(transform, st, en));
    }
    private IEnumerator Co_Outline(Transform outline, float st, float en)
    {
        float curTime;
        float durTime;
        while (true)
        {
            curTime = 0f;
            durTime = 0.2f;
            while (curTime < 1f)
            {
                Util.lossyscale(outline, outline.parent, EasingFunction.EaseOutCirc(st, en, Mathf.Clamp01(curTime += Time.deltaTime / durTime)));
                yield return null;
            }
            curTime = 0f;
            durTime = 0.4f;
            while (curTime < 1f)
            {
                Util.lossyscale(outline, outline.parent, EasingFunction.EaseInCirc(en, st, Mathf.Clamp01(curTime += Time.deltaTime / durTime)));
                yield return null;
            }
            yield return null;
        }
    }
}
