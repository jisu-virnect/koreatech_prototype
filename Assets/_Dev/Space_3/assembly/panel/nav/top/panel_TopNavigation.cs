using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class panel_TopNavigation : panel_Base
{
    private int stepIdx = -1;
    public view_Step[] steps;
    private Image img_Arrow;
    private GameObject go_Step_2;
    protected override void Awake()
    {
        base.Awake();
        img_Arrow = gameObject.Search<Image>(nameof(img_Arrow));
        go_Step_2 = gameObject.SearchGameObject(nameof(go_Step_2));
    }

    private void Start()
    {
        ResetStep();
    }

    public override void SetData<T>(T t)
    {
        base.SetData(t);
        Section section = t as Section;
        steps[0].SetText(section.step1);
        steps[1].SetText(section.step2);

        go_Step_2.SetActive(section.step2 == "" ? false : true);
        img_Arrow.gameObject.SetActive(section.step2 == "" ? false : true);
    }
    protected override IEnumerator Action_Opening()
    {
        yield return null;
        Util.RefreshLayout(gameObject, "img_BG");
    }

    public void NextStep()
    {
        SoundManager.instance.PlayEffect(eAudioClips.effect_screen);
        if (stepIdx >= steps.Length-1)
        {
            return;
        }
        ClearStep();
        if (stepIdx < steps.Length)
        {
            stepIdx++;
            steps[stepIdx].SetActive(eActive.active);
        }
    }

    private void ClearStep()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].SetActive(eActive.idle);
        }
    }

    public void ResetStep()
    {
        stepIdx = -1;
        ClearStep();
    }
}
