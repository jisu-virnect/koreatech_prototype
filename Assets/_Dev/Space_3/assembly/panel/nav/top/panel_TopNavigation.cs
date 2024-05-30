using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class panel_TopNavigation : panel_Base
{
    private int stepIdx = -1;
    public view_Step[] steps;

    private void Start()
    {
        ResetStep();
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

    public void ClearStep()
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
