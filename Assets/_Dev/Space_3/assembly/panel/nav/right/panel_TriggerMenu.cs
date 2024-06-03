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

    private eSectionType eSectionType;

    protected override void Awake()
    {
        base.Awake();

        img_Title = gameObject.Search<Image>(nameof(img_Title));
        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));
        tmp_Content = gameObject.Search<TMP_Text>(nameof(tmp_Content));

        btn_StartContent = gameObject.Search<Button>(nameof(btn_StartContent));
        btn_StartContent.onClick.AddListener(OnClick_StartContent);
    }

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
        UIManager.instance.GetPanel<panel_TopNavigation>().NextStep();
        switch (eSectionType)
        {
            case eSectionType.before:
                {
                    UIManager.instance.OpenPanel<panel_Check_Environment>(Define.before);
                }
                break;
            case eSectionType.install:
                {
                    UIManager.instance.OpenPanel<panel_Install>(Define.before);
                }
                break;
            case eSectionType.after:
                {
                    UIManager.instance.OpenPanel<panel_Check_Install>(Define.before);
                }
                break;
            default:
                break;
        }
    }
}
