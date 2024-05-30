using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class panel_TriggerMenu : panel_Base
{
    private TMP_Text tmp_Title;
    private Button btn_StartContent;
    protected override void Awake()
    {
        base.Awake();

        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));

        btn_StartContent = gameObject.Search<Button>(nameof(btn_StartContent));
        btn_StartContent.onClick.AddListener(OnClick_StartContent);
    }
    private eSectionType eSectionType;

    public override void SetData<T>(T t)
    {
        base.SetData(t);
        Section section = t as Section;
        eSectionType = Util.String2Enum<eSectionType>(section.type);
        tmp_Title.text = section.title;
        Util.RefreshLayout(gameObject, "go_Root2");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            Util.RefreshLayout(gameObject, "go_Root2");
        }
    }

    private void OnClick_StartContent()
    {
        Close();
        switch (eSectionType)
        {
            case eSectionType.before:
                UIManager.instance.GetPanel<panel_TopNavigation>().NextStep();
                UIManager.instance.OpenPanel<panel_Check_Environment>(Define.before);
                UIManager.instance.OpenPanel<panel_PlanMap>(Define.before);
                break;
            case eSectionType.install:
                UIManager.instance.OpenPanel<panel_Install>();
                break;
            case eSectionType.after:
                UIManager.instance.OpenPanel<panel_Install>();
                break;
            default:
                break;
        }
    }
}
