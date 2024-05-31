using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class popup_Success : popup_Base
{
    private TMP_Text tmp_Title;
    private TMP_Text tmp_Content;
    private Button btn_Confirm;
    private Action act;

    public virtual void SetAction(Action act)
    {
        this.act = act;
    }

    protected override void Awake()
    {
        base.Awake();
        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));
        tmp_Content = gameObject.Search<TMP_Text>(nameof(tmp_Content));
        btn_Confirm = gameObject.Search<Button>(nameof(btn_Confirm));
        btn_Confirm.onClick.AddListener(OnClick_Confirm);
    }

    private void OnClick_Confirm()
    {
        act?.Invoke();
        act = null;
        Close();
    }

    public override void SetData<T>(T t)
    {
        base.SetData(t);
        packet_popup_Basic packet_popup_Basic = t as packet_popup_Basic;
        tmp_Title.text = packet_popup_Basic.title;
        tmp_Content.text = packet_popup_Basic.content;
    }
}
