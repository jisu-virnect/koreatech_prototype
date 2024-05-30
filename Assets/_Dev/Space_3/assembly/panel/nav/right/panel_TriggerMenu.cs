using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel_TriggerMenu : panel_Base
{
    protected override void Awake()
    {
        base.Awake();
        Button btn_Checklist = gameObject.Search<Button>(nameof(btn_Checklist));
        btn_Checklist.onClick.AddListener(OnClick_Checklist);
        Button btn_Install = gameObject.Search<Button>(nameof(btn_Install));
        btn_Install.onClick.AddListener(OnClick_Install);
    }

    private void OnClick_Install()
    {
        Close();
        UIManager.instance.OpenPanel<panel_Install>();
    }

    private void OnClick_Checklist()
    {
        Close();
        UIManager.instance.OpenPanel<panel_Check_Environment>();
    }
}
