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
    private TMP_Text tmp_Content;
    private Image img_Title;
    private Button btn_StartContent;
    protected override void Awake()
    {
        base.Awake();

        img_Title = gameObject.Search<Image>(nameof(img_Title));
        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));
        tmp_Content = gameObject.Search<TMP_Text>(nameof(tmp_Content));

        btn_StartContent = gameObject.Search<Button>(nameof(btn_StartContent));
        btn_StartContent.onClick.AddListener(OnClick_StartContent);
    }
    private eSectionType eSectionType;

    public override void SetData<T>(T t)
    {
        base.SetData(t);
        Section section = t as Section;
        eSectionType = Util.String2Enum<eSectionType>(section.type);
        img_Title.sprite = ResourceManager.instance.LoadDataSprite(section.title_image);
        tmp_Title.text = section.title;
        tmp_Content.text = section.content;
    }


    private void OnClick_StartContent()
    {
        Close();
        switch (eSectionType)
        {
            case eSectionType.before:
                UIManager.instance.GetPanel<panel_TopNavigation>().NextStep();

                UIManager.instance.ShowToast<toast_Base>("화면 우측에서 작업현장 적합성을 확인합니다.")
                    .SetData(new packet_toast_basic(eToastColor.blue, eToastIcon.toast_idle));
                
                UIManager.instance.OpenPanel<panel_PlanMap>(Define.before);
                UIManager.instance.OpenPanel<panel_Check_Environment>(Define.before);
                break;
            case eSectionType.install:
                UIManager.instance.OpenPanel<panel_Install>();
                break;
            case eSectionType.after:
                //UIManager.instance.OpenPanel<panel_Install>();
                break;
            default:
                break;
        }
    }
}
