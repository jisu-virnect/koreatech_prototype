using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eToastColor
{
    blue,
    green, 
    red,
}
public enum eToastIcon
{
    toast_idle,
    toast_success,
}
public class toast_Basic : toast_Base
{
    private packet_toast_basic packet_Toast_Basic;
    private Image img_BG;
    private Image img_Icon;

    protected override void Awake()
    {
        base.Awake();
        img_BG = gameObject.Search<Image>(nameof(img_BG));
        img_Icon = gameObject.Search<Image>(nameof(img_Icon));

        packet_Toast_Basic = new packet_toast_basic(eToastColor.blue, eToastIcon.toast_idle);
    }

    public override void SetData<T>(T t)
    {
        base.SetData(t);
        packet_Toast_Basic = t as packet_toast_basic;
        SetToastBasicAddOn();
    }

    private void SetToastBasicAddOn()
    {
        switch (packet_Toast_Basic.eToastColor)
        {
            case eToastColor.blue: img_BG.color = Define.blue_idle; break;
            case eToastColor.green: img_BG.color = Define.green_success; break;
            case eToastColor.red: img_BG.color = Define.red; break;
            default: break;
        }

        img_Icon.sprite = ResourceManager.instance.LoadDataSprite(packet_Toast_Basic.eToastIcon.ToString());
    }
}
public class packet_toast_basic
{
    public eToastColor eToastColor;
    public eToastIcon eToastIcon;

    public packet_toast_basic(eToastColor eToastColor, eToastIcon eToastIcon)
    {
        this.eToastColor = eToastColor;
        this.eToastIcon = eToastIcon;
    }
}
