using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class panel_TopNavigation : panel_Base
{
    private int stepIdx = -1;
    public view_Step[] steps;

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
    }
    protected override IEnumerator Action_Opening()
    {
        yield return null;
        Util.RefreshLayout(gameObject, "img_BG");
    }

    public void NextStep()
    {
        if(stepIdx >= steps.Length-1)
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
